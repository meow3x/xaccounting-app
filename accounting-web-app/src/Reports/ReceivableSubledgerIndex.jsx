import { Divider, Text } from "@mantine/core";
import { usePagination } from "@mantine/hooks";
import { DataTableWrapper, Peso } from "src/util/table/common";
import { usePaginationState } from "src/util/table/pagination";
import { useGetReceivableReport } from "./api";

const columns = [
  {
    accessor: 'customerId'
  },
  {
    accessor: 'name'
  },
  {
    accessor: 'debit',
    render: (row) => Peso(row.debit)
  },
  {
    accessor: 'credit',
    render: (row) => Peso(row.credit)
  },
  {
    accessor: 'receivable',
    render: (row) => Peso(row.receivable)
  }
]

export default function ReceivableSubledgerIndex() {
  const paginationState = usePaginationState([10, 20, 50, 100])
  const { data: receivables, isFetching } = useGetReceivableReport()

  return (
    <>
      <Text size="lg" fw={500}>Accounts Receivable</Text>
      <Text mb="md" size="sm">As of {(new Date()).toLocaleDateString()}</Text>

      <DataTableWrapper
        columns={columns}
        records={receivables}
        totalRecords={receivables?.length}
        paginationState={paginationState}
        isFetching={isFetching}
        idAccessor="customerId"
      />
    </>
  )
}