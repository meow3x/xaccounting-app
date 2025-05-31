import { Group, Select, Stack, Table, Textarea, TextInput, Text, NumberInput, Divider, Button, Fieldset, ActionIcon, ScrollArea} from "@mantine/core";
import { DateInput } from "@mantine/dates";
import { useForm } from "@mantine/form";
import { IconCheck, IconPlus, IconSend, IconSquareCheckFilled, IconTrash, IconX } from "@tabler/icons-react";
import { useGetSuppliers } from "src/Suppliers/api";
import { FormatAddress, Peso } from "src/util/table/common";
import { useState } from "react";
import { useGetAccounts } from "src/ChartOfAccount/api";
import { useGetCostCenters } from "src/CostCenters/api";
import { useCreateApVoucher } from "./api";
import { showSuccessNotification } from "src/util/notification/notifications";
import { useQueryClient } from "@tanstack/react-query";

export function makeAccountsPayableForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      supplierId: null,
      referenceNumber: null,
      description: null,
      dueDate: null,
      debit: null,
      credit: null,
      accountId: null
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

export default function AccountsPayableForm({onClose}) {
  const form = useForm(makeAccountsPayableForm())
  const { data: accounts } = useGetAccounts({
    pagination: {
      page: 1,
      pageSize: 999999 // Load all accounts
    }
  })
  const { data: suppliers } = useGetSuppliers();
  const { data: costCenters } = useGetCostCenters();
  const [selectedSupplier, setSelectedSupplier] = useState(null); // For displaying supplier details
  const [journalLines, setJournalLines] = useState([])
  const mutation = useCreateApVoucher()
  const queryClient = useQueryClient()

  form.watch('supplierId', ({previousValue, value}) => {
    if (value !== previousValue) {
      const supplier = suppliers?.find(e => e.id === parseInt(value))
      setSelectedSupplier(supplier)
    }
  })

  function handleRowDelete(index) {
    setJournalLines([
      ...journalLines.slice(0, index),
      ...journalLines.slice(index + 1)
    ])
  }

  function handleSubmit() {
    const formData = form.getValues()
    // transform data
    const request = {
      supplierId: parseInt(formData.supplierId),
      referenceNumber: formData.referenceNumber,
      dueDate: new Date(formData.dueDate).toISOString().split('T')[0],
      lines: journalLines.map(e => ({
        accountId: e.account.id,
        description: e.description,
        debit: e.debit,
        credit: e.credit,
        costCenterId: e.costCenter.id
      }))
    }

    mutation.mutate(request, {
      onSuccess: (response) => {
        showSuccessNotification(`Accounts payable created with voucher number ${response.voucherNumber}`)
        form.reset()
        setJournalLines([])
        setSelectedSupplier(null)

        queryClient.invalidateQueries({ queryKey: [ 'accounts-payable' ]})
        onClose?.()
      }
    })
  }

  function handleAdd(selection) {
    // const selection = form.getValues()
    const account = accounts.records.find(e => e.id == selection.accountId)
    const costCenter = costCenters.find(e => e.id == selection.costCenterId)

    const update = [
      ...journalLines,
      {
        account: { ...account },
        description: selection.description,
        costCenter: { ...costCenter },
        referenceNumber: selection.referenceNumber,
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
          <DateInput
            readOnly
            variant="filled"
            defaultValue={new Date()}
            label="Date"
          />

          <Select
            required
            label="Supplier"
            placeholder="Select Supplier"
            searchable
            checkIconPosition="right"
            data={suppliers?.map(e => ({
              value: e.id.toString(),
              label: `${e.supplierId} - ${e.name}`
            }))}
            key={form.key('supplierId')}
            {...form.getInputProps('supplierId')}
          />

          {selectedSupplier ? <SupplierBasicDetails supplier={selectedSupplier} /> : null}

          <TextInput
            label="Reference Number"
            key={form.key('referenceNumber')}
            {...form.getInputProps('referenceNumber')}
          />

          <DateInput
            required
            label="Due Date"
            placeholder="Select Due Date"
            key={form.key('dueDate')}
            {...form.getInputProps('dueDate')}
          />

          <Divider mt="md" variant="dashed" />

          <Select
            required
            label="Cost Center"
            placeholder="Select Cost Center"
            searchable
            checkIconPosition="right"
            data={costCenters?.map(e => ({
              value: e.id.toString(),
              label: e.name
            }))}
            key={form.key('costCenterId')}
            {...form.getInputProps('costCenterId')}
          />

          <Textarea
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
          <JournalEntryTable
            lines={journalLines}
            onRowDelete={handleRowDelete}/>
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

export function SupplierBasicDetails({supplier}) {
  return (
    <Table variant="vertical" layout="fixed" withTableBorder >
      <Table.Tbody>
        <Table.Tr>
          <Table.Th w={160}>Balance</Table.Th>
          <Table.Td>{Peso(supplier?.balance)}</Table.Td>
        </Table.Tr>

        <Table.Tr>
          <Table.Th>TIN</Table.Th>
          <Table.Td>{supplier?.tin ?? '-'}</Table.Td>
        </Table.Tr>

        <Table.Tr>
          <Table.Th>Address</Table.Th>
          <Table.Td>{FormatAddress(supplier?.address)}</Table.Td>
        </Table.Tr>

        <Table.Tr>
          <Table.Th>Terms</Table.Th>
          <Table.Td>{supplier?.paymentTerm?.label}</Table.Td>
        </Table.Tr>
      </Table.Tbody>
    </Table>
  )
}

export function JournalEntryTable({lines, withCostCenter = true, onRowDelete}) {
  const totalDebit = lines.map(e => e.debit).reduce((a, c) => a + c, 0)
  const totalCredit = lines.map(e => e.credit).reduce((a, c) => a + c, 0)
  const isBalanced = totalDebit - totalCredit == 0.0

  return (
    <>
      <Text size="lg" fw={600}>Journal Entry</Text>

      <Table highlightOnHover withColumnBorders>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Reference #</Table.Th>
            <Table.Th>Description</Table.Th>
            {withCostCenter ? <Table.Th>Cost Center</Table.Th> : null}
            <Table.Th w={150}>Account ID</Table.Th>
            <Table.Th>Account Name</Table.Th>
            <Table.Th>Debit</Table.Th>
            <Table.Th>Credit</Table.Th>
            <Table.Th w={50}></Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {lines.map((e, i) =>
            <Table.Tr key={i}>
              <Table.Td>{e.referenceNumber}</Table.Td>
              <Table.Td>{e.description}</Table.Td>
              {withCostCenter ? <Table.Td>{e.costCenter.name}</Table.Td> : null }
              <Table.Td>{e.account.accountId}</Table.Td>
              <Table.Td>{e.account.name}
                &nbsp; <Text component="small" size="sm" fs="italic" c="dimmed">({e.account.accountType?.name ?? 'unknown'})</Text>
              </Table.Td>
              <Table.Td>{Peso(e.debit)}</Table.Td>
              <Table.Td>{Peso(e.credit)}</Table.Td>
              <Table.Td>
                <ActionIcon size="sm" variant="subtle" color="red" onClick={() => onRowDelete?.(i)}>
                  <IconX size={16} />
                </ActionIcon>
              </Table.Td>
            </Table.Tr>
          )}
          <Table.Tr >
            <Table.Td colSpan={withCostCenter ? 5 : 4} fw={500}>Total</Table.Td>
            <Table.Td fw={500}>{Peso(totalDebit)}</Table.Td>
            <Table.Td fw={500}>{Peso(totalCredit)}</Table.Td>
            <Table.Td></Table.Td>
          </Table.Tr>
          <Table.Tr >
            <Table.Td colSpan={withCostCenter ? 5 : 4}>Balance</Table.Td>
            <Table.Td  c={isBalanced ? '' : 'red.5'} fw={500}>
              {isBalanced ? '-'  : Peso(totalDebit - totalCredit)}
            </Table.Td>
            <Table.Td></Table.Td>
            <Table.Td></Table.Td>
          </Table.Tr>
        </Table.Tbody>
      </Table>
    </>
  )
}

export function JournalEntrySimple({lines}) {
  const totalDebit = lines.map(e => e.debit).reduce((a, c) => a + c, 0)
  const totalCredit = lines.map(e => e.credit).reduce((a, c) => a + c, 0)

  return (
    <>
      <Table highlightOnHover withColumnBorders>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Description</Table.Th>
            <Table.Th>Cost Center</Table.Th>
            <Table.Th w={150}>Account ID</Table.Th>
            <Table.Th>Account Name</Table.Th>
            <Table.Th>Debit</Table.Th>
            <Table.Th>Credit</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {lines.map((e, i) =>
            <Table.Tr key={i}>
              <Table.Td>{e.description}</Table.Td>
              <Table.Td>{e.costCenter.name}</Table.Td>
              <Table.Td>{e.account.accountId}</Table.Td>
              <Table.Td>{e.account.name}
                &nbsp; <Text component="small" size="sm" fs="italic" c="dimmed">({e.account.accountType?.name ?? 'unknown'})</Text>
              </Table.Td>
              <Table.Td>{Peso(e.debit)}</Table.Td>
              <Table.Td>{Peso(e.credit)}</Table.Td>
            </Table.Tr>
          )}
          <Table.Tr >
            <Table.Td colSpan={4} fw={500}>Total</Table.Td>
            <Table.Td fw={500}>{Peso(totalDebit)}</Table.Td>
            <Table.Td fw={500}>{Peso(totalCredit)}</Table.Td>
          </Table.Tr>
        </Table.Tbody>
      </Table>
    </>
  )
}
