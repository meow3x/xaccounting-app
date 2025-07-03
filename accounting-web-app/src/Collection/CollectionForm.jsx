import { Button, Divider, Group, Notification, NumberInput, Select, Stack, Table, Textarea, TextInput } from "@mantine/core";
import { useGetInvoiceOnCreditByNumber, usePostCollectorPayment } from "./api";
import { useGetAccounts } from "src/ChartOfAccount/api";
import { useEffect, useState } from "react";
import { useForm } from "@mantine/form";
import { JournalEntryTable } from "src/AccountsPayable/AccountsPayableForm";
import { IconCheck, IconPlus, IconX } from "@tabler/icons-react";
import { useQueryClient } from "@tanstack/react-query";
import { showSuccessNotification } from "src/util/notification/notifications";

function makeCollectionForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      description: null,
      invoiceNumber: null,
      accountId: null,
      debit: null,
      credit: null
    }
  }
}
export default function CollectionForm({invoiceNumber, onClose}) {
  const { data: invoice, isFetching } = useGetInvoiceOnCreditByNumber(invoiceNumber)
  const { data: accounts } = useGetAccounts({
    pagination: {
      page: 1,
      pageSize: 999999 // load all accounts
    },
  })
  const form = useForm(makeCollectionForm())
  const [journalLines, setJournalLines] = useState([])
  const mutation = usePostCollectorPayment()
  const queryClient = useQueryClient()

  useEffect(() => {
    if (invoice) {
      // Reverse the debit (Receivable)
      const debit = invoice.journalEntry.lines.filter(e => !!e.debit)
      const credits = invoice.journalEntry.lines.filter(e => !!e.credit).map(e => e.credit)
      const values = {
        description: `Cash Collection On Due Accounts`,
        // auto populate debit
        invoiceNumber: invoice.number,
        debit: credits.reduce((a, c) => a + c, 0)
      }

      // pre-populate credit on receivable
      const lines = debit.map(e => ({
        account: { ...e.account },
        description: e.description,
        costCenter: { ...e.costCenter },
        referenceNumber: invoice.number.toString(),
        credit: e.debit, // Reverse
        debit: null
      }))

      setJournalLines(lines)

      form.setValues(values)
      form.resetDirty(values)
    }
  }, [invoice])

  function handleSubmit() {
    const formData = form.getValues()
    const request = {
      invoiceNumber: formData.invoiceNumber,
      referenceNumber: formData.referenceNumber,
      lines: journalLines.map(e => ({
        // No cost center for collections
        accountId: e.account.id,
        description: e.description,
        referenceNumber: e.referenceNumber,
        debit: e.debit,
        credit: e.credit
      }))
    }

    console.log(request)

    mutation.mutate(request, {
      onSuccess: (response) => {
        showSuccessNotification(`Sucesfully posted payment with receipt #${response.receiptNumber}`)
        form.reset()
        setJournalLines([])

        queryClient.invalidateQueries({ queryKey: ['invoice-on-account'] })

        onClose?.()
      }
    })
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
        <Stack flex={1}>
          <Table variant="vertical" layout="fixed">
            <Table.Tbody>
              <Table.Tr>
                <Table.Th>Date</Table.Th>
                <Table.Td>{(new Date()).toLocaleDateString()}</Table.Td>
              </Table.Tr>
              <Table.Tr>
                <Table.Th>Customer ID</Table.Th>
                <Table.Td>{invoice?.customer.customerId}</Table.Td>
              </Table.Tr>
              <Table.Tr>
                <Table.Th>Customer Name</Table.Th>
                <Table.Td>{invoice?.customer.name}</Table.Td>
              </Table.Tr>
            </Table.Tbody>
          </Table>

          {/* Account selection */}
          <TextInput
            required
            label="Reference Number"
            placeholder="Input reference number"
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
        <Stack flex={3}>
          <Notification withBorder withCloseButton={false} color="orange.4">
            Please select an account to debit
          </Notification>

          <JournalEntryTable lines={journalLines} withCostCenter={false} />

          <Group justify="right">
            <Button
              loading={form.submitting}
              onClick={handleSubmit}
              disabled={! (journalLines.length > 0 && isBalanced)}
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