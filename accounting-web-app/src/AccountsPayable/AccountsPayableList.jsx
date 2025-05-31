import { DataTableWrapper, Heading, Peso } from "src/util/table/common";
import { useGetAccountsPayable } from "./api";
import { usePaginationState } from "src/util/table/pagination";
import { Box, Button, Code, Container, Divider, em, Group, Modal, NumberInput, Paper, Stack, Text, TextInput } from "@mantine/core";
import { useState } from "react";
import { IconEyeDollar, IconPrinter, IconReceiptDollar } from "@tabler/icons-react";
import AccountsPayableForm, { JournalEntrySimple, JournalEntryTable } from "./AccountsPayableForm";
import { DateInput } from "@mantine/dates";
import { useDisclosure } from "@mantine/hooks";
import { useNavigate } from "react-router";

export function AccountsPayableList({onPaymentRequest}) {
  const { data, isFetching } = useGetAccountsPayable()
  const paginationState = usePaginationState([10, 20, 50, 100])
  const [selectedRow, setSelectedRow] = useState(null)
  const navigate = useNavigate()

  const colums = [
    {
      accessor: 'voucherNumber',
      title: 'Voucher #',
      // width: '10%'
    },
    {
      accessor: 'referenceNumber',
      title: 'Reference #'
    },
    // {
    //   accessor: 'createdAt',
    //   title: 'Date Created',
    //   render: (row) => (new Date(row.createdAt)).toLocaleDateString()
    // },
    {
      accessor: 'dueDate',
      title: 'Due Date',
      render: (row) => (new Date(row.dueDate)).toLocaleDateString()
    },
    {
      accessor: 'supplier.supplierId',
      title: 'Supplier ID'
    },
    {
      accessor: 'supplier.name',
      title: 'Supplier Name'
    },
    {
      accessor: 'amount',
      title: 'Amount',
      render: (row) => {
        return Peso(row.totalAmount)
      }
    },
    {
      accessor: 'paid',
      title: 'Paid',
      render: (row) => {
        return Peso(row.totalAmount - row.balance)
      }
    }
  ]

  return (
    <>
      <Stack>
        <Group grow>
          <DataTableWrapper
            columns={colums}
            records={data}
            totalRecords={data?.length}
            paginationState={paginationState}
            isFetching={isFetching}
            onRowClick={({record, index}) => {
              setSelectedRow(record)
            }}
          />
        </Group>

        {selectedRow ?
          <>
            <Text c="gray.6" size="md" fw={500}>Details - A/P #{selectedRow?.voucherNumber}</Text>
            <PayableDetails ap={selectedRow} onPaymentRequest={onPaymentRequest} />
          </>
          : <Text size="sm" ta="center" c="gray.5">Click on a row to view the details.</Text>}
      </Stack>
    </>
  )
}

export function PayableDetails({ap, onPaymentRequest}) {
  // const totalDebit = ap.journalEntry.lines.map(e => e.debit).reduce((a, c) => a + c, 0)

  return (
    <Paper shadow="md" withBorder>

      <Group align="flex-start" m="md">
        <Stack flex={1}>
          <NumberInput
            readOnly
            variant="filled"
            label="Voucher Number"
            value={ap.voucherNumber}
          />

          <DateInput
            readOnly
            variant="filled"
            label="Date Created"
            value={new Date(ap.createdAt.split('T')[0])}
          />

          <TextInput
            readOnly
            variant="filled"
            label="Supplier"
            value={`${ap.supplier.supplierId} - ${ap.supplier.name}`}
            // value={ap?.supplier.name}
          />

          {/* {selectedSupplier ? <SupplierBasicDetails supplier={selectedSupplier} /> : null} */}

          <TextInput
            readOnly
            variant="filled"
            label="Reference Number"
            value={ap.referenceNumber}
          />

          <DateInput
            readOnly
            variant="filled"
            label="Due Date"
            value={new Date(ap.dueDate)}
          />
        </Stack>

        <Stack flex={3}>
          <Group >
            <Paper shadow="lg" p={10} bg="dark.2" c="white" radius="md">
              <Text size="sm" mb="md">Payable Balance</Text>
              <Text fw={500} size="xl">{Peso(ap.balance)}</Text>
            </Paper>
          </Group>

          <Box>
            {/* <Text>Details</Text> */}
            <JournalEntrySimple lines={ap?.journalEntry.lines ?? []} />
          </Box>

          {/* <Box>
            <Text>Payment</Text>
            <JournalEntrySimple lines={ap?.journalEntry.lines ?? []} />

          </Box> */}
        </Stack>
        </Group>

      <Group justify="right" m="md">
        <Button disabled={ap.balance == 0.0} variant="outline" color="dark" >Print Voucher</Button>
        <Button
          onClick={() => onPaymentRequest?.(ap)}
          disabled={ap.balance == 0}
          variant="filled"
          color="blue">
            Pay {Peso(ap.balance)}
        </Button>
      </Group>
    </Paper>
  )
}
