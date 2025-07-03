import { Code, Table, Tabs, Text } from "@mantine/core";
import { useInventoryCostView, useInventoryQuantityView } from "./api";
import { DataTableWrapper, Peso } from "src/util/table/common";
import { usePagination } from "@mantine/hooks";
import { usePaginationState } from "src/util/table/pagination";

export default function InventoryIndex() {
  return (
    <>
      <Text size="lg" fw={500} mb="md">Inventory</Text>

      <Tabs orientation="vertical" color="green" defaultValue="quantity-view" variant="pills">
        <Tabs.List>
          <Tabs.Tab value="quantity-view">
            Material Inventory Quantity
          </Tabs.Tab>
          <Tabs.Tab value="cost-view">
            Ending Inventory Cost
          </Tabs.Tab>
          <Tabs.Tab value="per-category-cost-view">
            Per Category Cost
          </Tabs.Tab>
          <Tabs.Tab value="per-category-quantity">
            Per Category Quantity
          </Tabs.Tab>
          <Tabs.Tab value="period-view">
            Inventory Period
          </Tabs.Tab>
        </Tabs.List>

        <Tabs.Panel value="quantity-view">
          <QuantityView />
        </Tabs.Panel>
        <Tabs.Panel value="cost-view">
          <CostView />
        </Tabs.Panel>

      </Tabs>
      {/* <QuantityView data={inventory} isFetching={isFetching} /> */}
    </>
  )
}

export function QuantityView() {
  const paginationState = usePaginationState([10, 20, 50, 100], 1)
  const {data, isFetching} = useInventoryQuantityView()

  const columns = [
    {
      accessor: 'itemCode'
    },
    {
      accessor: 'name'
    },
    {
      accessor: 'quantity'
    }
  ]
  return (
    <DataTableWrapper
      columns={columns}
      records={data}
      totalRecords={data?.length}
      paginationState={paginationState}
      isFetching={isFetching}
    />
  )
}

export function CostView() {
  const paginationState = usePaginationState([10, 20, 50, 100], 1)
  const { data, isFetching } = useInventoryCostView()

  const columns = [
    {
      accessor: 'itemCode',
    },
    {
      accessor: 'itemName'
    },
    {
      accessor: 'purchase',
      render: (row) => Peso(row.purchase)
    },
    {
      accessor: 'sold',
      render: (row) => Peso(row.sold)
    },
    {
      accessor: 'endCost',
      render: (row) => Peso(row.endCost)
    }
  ]

  return (
    <DataTableWrapper
      columns={columns}
      records={data}
      totalRecords={data?.length}
      paginationState={paginationState}
      isFetching={isFetching}
      idAccessor={'itemCode'}
    />
  )
}