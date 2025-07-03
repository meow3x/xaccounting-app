import { DataTableWrapper, Peso } from "src/util/table/common";
import { useGetPayments } from "./api";
import { Group, Stack, Text } from "@mantine/core";
import { usePaginationState } from "src/util/table/pagination";
import { useState } from "react";
import DisbursementDetails from "./DisbursementDetails";

export default function DisbursementsList() {
  const { data: payments, isFetching } = useGetPayments()
  const paginationState = usePaginationState([10,20,50,100])
  const [selectedRow, setSelectedRow] = useState(null)

  const columns = [
    {
      accessor: 'createdAt',
      title: 'Date Created',
      render: (row) => (new Date(row.createdAt)).toLocaleDateString()
    },
    {
      accessor: 'voucherNumber',
      title: 'CV Number'
    },
    {
      accessor: 'referenceNumber',
      title: 'Reference / Check #'
    },
    {
      accessor: 'apvNumber',
      title: 'A/P Voucher #',
      render: (row) => row.apvNumber == null ? '-' : row.apvNumber
    },
    {
      accessor: 'payee.name'
    },
    {
      accessor: 'totalAmount',
      title: 'Total',
      render: (row) => Peso(row.totalAmount)
    },
    {
      accessor: 'chequeStatus',
      title: 'Check Status',
      render: (row) => row.chequeStatus == null
        ? '-'
        : (row.chequeStatus == 'Pending' ? 'Not Printed' : 'Printed')
    }
  ]

  return (
    <>
      <Stack>
        <Group grow>
          <DataTableWrapper
            columns={columns}
            records={payments}
            totalRecords={payments?.length}
            paginationState={paginationState}
            isFetching={isFetching}
            onRowClick={({record, index}) => {
              setSelectedRow(record)
            }} />

        </Group>
          {selectedRow ?
            <>
              <Text size="sm" mb="md">Details</Text>
              <DisbursementDetails payment={selectedRow} />
            </>
            : <Text size="sm" ta="center" c="gray.5">Click on a row to view the details.</Text> }
      </Stack>
    </>
  )
}
