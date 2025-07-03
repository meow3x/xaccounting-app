import { DataTableWrapper, Peso } from "src/util/table/common";
import { useGetInvoiceOnCredit } from "./api";
import { Box, Button, Divider, Group, Paper, Stack, Table, Text } from "@mantine/core";
import { usePaginationState } from "src/util/table/pagination";
import { useState } from "react";
import { JournalEntrySimple } from "src/AccountsPayable/AccountsPayableForm";
import { IconPrinter } from "@tabler/icons-react";

const columns = [
  {
    accessor: 'number',
    title: 'Invoice #'
  },
  {
    accessor: 'createdAt',
    render: (row) => (new Date(row.createdAt)).toLocaleString()
  },
  {
    accessor: 'paymentMethod'
  },
  {
    accessor: 'customer',
    render: (row) => `${row.customer.customerId} - ${row.customer.name}`
  },
  {
    accessor: 'netAmount',
    render: (row) => Peso(row.netAmount)
  },
  {
    accessor: 'paid',
    title: 'Paid',
    render: (row) => Peso(row.netAmount - row.instalmentBalance)
  },
  {
    accessor: 'instalmentBalance',
    title: 'Balance',
    render: (row) => Peso(row.instalmentBalance)
  }
]

export default function CollectionCasesList({onPaymentRequest}) {
  const { data: onAccounts, isFetching } = useGetInvoiceOnCredit()
  const paginationState = usePaginationState([10, 20, 50, 100])
  const [selectedRow, setSelectedRow] = useState(null)

  return (
    <>
      <Stack>
        <Group grow>
          <DataTableWrapper
            columns={columns}
            records={onAccounts}
            totalRecords={onAccounts?.length}
            paginationState={paginationState}
            isFetching={isFetching}
            onRowClick={({record}) => {
              setSelectedRow(record)
            }}
          />
        </Group>

        {selectedRow ?
          <>
            <Text>Details</Text>
            <InvoiceDetails invoice={selectedRow} onPaymentRequest={onPaymentRequest}/>
            {selectedRow?.payments?.length > 0
              ? <>
                  <Text>Payments</Text>
                  <CollectionTrail invoice={selectedRow} />
                </>
              : null}
          </>
          : <Text size="sm" ta="center" c="gray.5">Click on a row to view the details.</Text>}
      </Stack>
    </>
  )
}

// FIXME: Choose a canonical name for "on account" purchases
export function InvoiceDetails({invoice, onPaymentRequest}) {
  return (
    <Paper shadow="sm" withBorder>
      <Group m="sm">
        <JournalEntrySimple lines={invoice?.journalEntry.lines ?? []} label="" />
      </Group>
      <Group justify="right" m="md">
        <Button
          disabled={invoice.instalmentBalance <= 0}
          onClick={() => onPaymentRequest?.(invoice)}
          color="blue">Pay {Peso(invoice.instalmentBalance)}</Button>
      </Group>
    </Paper>
  )
}

export function CollectionTrail({invoice}) {
  if ((invoice?.payments.length ?? 0) <= 0) return <Text size="sm">No payment record yet</Text>

  return (
    <Paper shadow="sm" withBorder>
      {invoice.payments.map(e =>
        <Group m="sm" align="flex-start" key={e.id}>
          <Stack flex={1}>
            <Table variant="vertical" withTableBorder>
              <Table.Tbody>
                <Table.Tr>
                  <Table.Th>Receipt #</Table.Th>
                  <Table.Td>{e.receiptNumber}</Table.Td>
                </Table.Tr>
                <Table.Tr>
                  <Table.Th>Paid On</Table.Th>
                  <Table.Td>{(new Date(e.createdAt)).toLocaleString()}</Table.Td>
                </Table.Tr>
                <Table.Tr>
                  <Table.Th>Reference #</Table.Th>
                  <Table.Td>{e.referenceNumber}</Table.Td>
                </Table.Tr>
              </Table.Tbody>
            </Table>


          </Stack>
          <Stack flex={3}>
            <JournalEntrySimple lines={e.journalEntry.lines} label={null} />

            <Group justify="right">
              <Button variant="outline" color="dark" rightSection={<IconPrinter size={16} />}>
                Print
              </Button>
            </Group>
            <Divider variant="dotted" />

          </Stack>

        </Group>)
      }
    </Paper>
  )
}