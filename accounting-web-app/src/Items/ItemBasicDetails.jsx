import { Loader, Table } from "@mantine/core";
import { Peso } from "src/util/table/common";

export default function ItemBasicDetails({item, quantityBalance, isSelling = true}) {
  return (
    <Table variant="vertical" layout="fixed" withTableBorder>
      <Table.Tbody>
        <Table.Tr fw={600} c={(quantityBalance ?? 0) == 0 ? 'red.7' : 'green.6'}>
          <Table.Th>Quantity Balance</Table.Th>
          <Table.Td>{quantityBalance}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>Measurement</Table.Th>
          <Table.Td>{item?.uom.name ?? '-'}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>Category</Table.Th>
          <Table.Td>{item?.category.name ?? '-'}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>{isSelling ? 'Unit Price' : 'Unit Cost'}</Table.Th>
          <Table.Td>{Peso(isSelling ? item?.unitPrice : item?.unitCost)}</Table.Td>
        </Table.Tr>
      </Table.Tbody>
    </Table>
  )
}