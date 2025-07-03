import { Text } from "@mantine/core";
import { useGetPayableReport } from "./api";
import { DataTableWrapper, Peso } from "src/util/table/common";
import { usePaginationState } from "src/util/table/pagination";


const columns = [
  {
    accessor: 'supplierId'
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
    accessor: 'payable',
    render: (row) => Peso(row.payable)
  }
]


export default function PayableSubledgerIndex() {
  const paginationState = usePaginationState([10, 20, 50, 100])
  const { data: receivables, isFetching } = useGetPayableReport()

  return (
    <>
      <Text size="lg" fw={500}>Accounts Payable</Text>
      <Text mb="md" size="sm">As of {(new Date()).toLocaleDateString()}</Text>

      <DataTableWrapper
        columns={columns}
        records={receivables}
        totalRecords={receivables?.length}
        paginationState={paginationState}
        isFetching={isFetching}
        idAccessor="supplierId"
      />
    </>
  )
}