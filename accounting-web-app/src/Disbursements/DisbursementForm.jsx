import { Button, Checkbox, Divider, Group, Notification, NumberInput, Select, Stack, Textarea, TextInput } from "@mantine/core";
import { DateInput } from "@mantine/dates";
import { useForm } from "@mantine/form";
import { IconAlertCircleFilled, IconAlertTriangle, IconCheck, IconPlus, IconX } from "@tabler/icons-react";
import { useEffect, useState } from "react";
import { JournalEntryTable } from "src/AccountsPayable/AccountsPayableForm";
import { useGetPayable, useGetVouchers } from "src/AccountsPayable/api";
import { useGetAccounts } from "src/ChartOfAccount/api";
import { useGetCostCenters } from "src/CostCenters/api";
import { useGetSuppliers } from "src/Suppliers/api";
import { usePostPayment } from "./api";
import { useQueryClient } from "@tanstack/react-query";
import { showSuccessNotification } from "src/util/notification/notifications";

export function makeDisbursementForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      apvNumber: null,
      payeeId: null,
      isCheque: true,
      referenceNumber: null,
      description: null,
      accountId: null,
      debit: null,
      credit: null
    },
    validate: {
      debit: (value, values) => {
        // at least debit or credit required, but not both
        if (!value && !values.credit ||
          value && values.credit) {
          return 'Debit or credit required. But not both'
        }
        return null
      },
      credit: (value, values) => {
        if (!value && !values.debit ||
          value && values.debit) {
          return 'Debit or credit required. But not both'
        }
        return null
      }
    }
  }
}

export default function DisbursementForm({apVoucherNumber, onClose}) {
  const form = useForm(makeDisbursementForm())
  const { data: suppliers } = useGetSuppliers()
  const { data: accounts } = useGetAccounts()
  const [journalLines, setJournalLines] = useState([])
  const { data: payable, isFetching } = useGetPayable(apVoucherNumber)
  const { data: vouchers } = useGetVouchers()
  const { data: costCenters } = useGetCostCenters()
  const mutation = usePostPayment()
  const queryClient = useQueryClient()

  useEffect(() => {
    if (payable) {
      // Reverse credits
      const credits = payable.journalEntry.lines.filter(e => !!e.credit)
      const debits = payable.journalEntry.lines.filter(e => !!e.debit).map(e => e.debit)

      // set form values
      const values = {
        apvNumber: payable.voucherNumber.toString(),
        payeeId: payable.supplier.id.toString(),
        description: `PAYMENT FOR: ${credits[0].description}`,
        // debit journal line is already pre-populated.
        credit: debits.reduce((a, c) => a + c, 0)
      }


      const lines = credits.map(e => ({
        account: { ...e.account },
        description: e.description,
        costCenter: { ...e.costCenter },
        referenceNumber: payable.voucherNumber.toString(),
        debit: e.credit, // reverse
        credit: null
      }))

      setJournalLines(lines)

      form.setValues(values)
      form.resetDirty(values)
    }
  }, [payable])

  function handleSubmit() {
    const formData = form.getValues()
    // Tranform data
    const request = {
      payeeId: parseInt(formData.payeeId),
      isCheque: formData.isCheque,
      referenceNumber: formData.referenceNumber,
      apVoucherNumber: formData.apvNumber,
      lines: journalLines.map(e => ({
        // No cost center for disbursement
        accountId: e.account.id,
        description: e.description,
        referenceNumber: e.referenceNumber,
        debit: e.debit,
        credit: e.credit
      }))
    }

    mutation.mutate(request, {
      onSuccess: (response) => {
        showSuccessNotification(`You have added a new disbursement entry with voucher number ${response.voucherNumber}`)

        form.reset()
        setJournalLines([])

        queryClient.invalidateQueries({ queryKey: ['disbursements']})
        queryClient.invalidateQueries({queryKey: ['accounts-payable']})
        onClose?.()
      }
    })
  }

  function handleRowDelete(index) {
    setJournalLines([
      ...journalLines.slice(0, index),
      ...journalLines.slice(index + 1)
    ])
  }

  function handleAdd(selection) {
    const account = accounts.records.find(e => e.id == selection.accountId)
    const update = [
      ...journalLines,
      {
        account: { ...account },
        description: selection.description,
        referenceNumber: selection.referenceNumber,
        costCenterId: null, // No cost center for disbursements
        debit: !!selection.debit ? parseFloat(selection.debit) : null,
        credit: !!selection.credit ? parseFloat(selection.credit) : null
      }
    ]
    setJournalLines(update)
  }

  const totalDebit = journalLines.map(e => e.debit).reduce((a, c) => a + c, 0)
  const totalCredit = journalLines.map(e => e.credit).reduce((a, c) => a + c, 0)
  const isBalanced = totalDebit - totalCredit == 0.0

  return (
    <form onSubmit={form.onSubmit(handleAdd)}>
      <Group align="flex-start">
        {/* form */}
        <Stack flex={1}>
          <DateInput
            readOnly
            variant="filled"
            defaultValue={new Date()}
            label="Date" />

          <Select
            searchable
            readOnly={!!apVoucherNumber}
            variant={!!apVoucherNumber ? 'filled' : 'default'}
            label="A/P Voucher Number"
            data={vouchers?.map(e => e.toString())}
            key={form.key('apvNumber')}
            {...form.getInputProps('apvNumber')}
          />

          <Select
            required
            label="Name / Payee"
            placeholder="Select Payee"
            searchable
            checkIconPosition="right"
            data={suppliers?.map(e => ({
              value: e.id.toString(),
              label: `${e.supplierId} - ${e.name}`
            }))}
            key={form.key('payeeId')}
            {...form.getInputProps('payeeId')}
          />

          <Checkbox
            required
            defaultChecked
            label="Cheque Payment"
            key={form.key('isCheque')}
            {...form.getInputProps('isCheque')}
          />

          <TextInput
            required
            label="Reference Number"
            placeholder="Cheque number, etc..."
            key={form.key('referenceNumber')}
            {...form.getInputProps('referenceNumber')}
          />

          <Divider variant="dashed" />

          <Textarea
            required
            label="Description"
            minLength={3}
            rows={3}
            key={form.key('description')}
            {...form.getInputProps('description')}
          />

          <Select
            required
            label="Account"
            placeholder="Select Account"
            searchable
            checkIconPosition="right"
            data={accounts?.records.map(e => ({
              value: e.id.toString(),
              label: `${e.accountId} - ${e.name} (${e.accountType.name})`
            }))}
            key={form.key('accountId')}
            {...form.getInputProps('accountId')}
          />

          <Group grow>
            <NumberInput
              min={0.1}
              label="Debit"
              key={form.key('debit')}
              {...form.getInputProps('debit')}
            />

            <NumberInput
              min={0.1}
              label="Credit"
              key={form.key('credit')}
              {...form.getInputProps('credit')}
            />
          </Group>

          <Group justify="right" mt="lg">
            <Button rightSection={<IconX size={16} />} variant="light" color="red">
              Clear
            </Button>
            <Button type="submit" rightSection={<IconPlus size={16} />} variant="light" >
              Add
            </Button>
          </Group>

        </Stack>

        {/* journal view */}
        <Stack flex={3}>
          <Notification withBorder withCloseButton={false} color="orange.4">
            Please select correct cash / check account
          </Notification>

          <JournalEntryTable lines={journalLines} withCostCenter={false} onRowDelete={handleRowDelete}/>

          <Group justify="right">
            <Button
              loading={form.submitting}
              onClick={handleSubmit}
              disabled={!(journalLines.length > 0 && isBalanced)}
              rightSection={<IconCheck size={16} />}
              variant="filled"
              color="green">
              Submit
            </Button>
          </Group>
        </Stack>
      </Group>

    </form>
  )
}