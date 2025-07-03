import { Table } from "@mantine/core";
import { Peso } from "src/util/table/common";


export default function SalesSummary({ sale }) {
  return (
    <Table variant="vertical" layout="fixed" withTableBorder>
      <Table.Tbody>
        <Table.Tr>
          <Table.Th>VATable Amount</Table.Th>
          <Table.Td>{Peso(sale?.vatableAmount)}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>VAT Amount</Table.Th>
          <Table.Td>{Peso(sale?.vatAmount)}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>Total Amount</Table.Th>
          <Table.Td>{Peso(sale?.netAmount)}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>Discount</Table.Th>
          <Table.Td>{Peso(0)}</Table.Td>
        </Table.Tr>
        <Table.Tr>
          <Table.Th>NET Amount</Table.Th>
          <Table.Td>{Peso(sale?.netAmount)}</Table.Td>
        </Table.Tr>
      </Table.Tbody>
    </Table>
  );
}
