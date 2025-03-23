import { Button, Divider, Group, Table, Text, Title , ActionIcon, Pagination} from "@mantine/core"
import { useState, useEffect } from "react";
import { IconCross, IconEdit, IconPlus, IconTrash, IconX } from "@tabler/icons-react"
import axios from "axios";

export default function AccountTypesIndex() {
  const headers = [ "ID", "Account Type", "Controls" ]
  const [accountTypes, setAccountTypes] = useState([])

  const loadAccountTypes = () => {
    return axios.get(import.meta.env.VITE_API_ACCOUNT_TYPES).then(response => {
      setAccountTypes(response.data)
    })
  }

  useEffect(() => {
    loadAccountTypes()
  }, [])

  return (
    <>
     <Group justify="right">
        <Button rightSection={<IconPlus size={14} />}>Create Type</Button>
      </Group>

      <Table highlightOnHover withColumnBorders withRowBorders={true} striped captionSide="bottom">
        <Table.Thead>
          <Table.Tr>
            {headers.map(e => <Table.Th key={e}>{e}</Table.Th>)}
          </Table.Tr>
        </Table.Thead>

        <Table.Tbody>
          {accountTypes.map(e =>
            <Table.Tr key={e.id}>
              <Table.Td>{e.id}</Table.Td>
              <Table.Td>{e.label}</Table.Td>
              <Table.Td>
                <Button.Group>
                  <Button
                    size="sm"
                    variant="light"
                    leftSection={<IconEdit size={14}/>}
                  >
                      Update
                  </Button>
                  <Button size="sm"variant="light" color="red" leftSection={<IconTrash size={14}/>}>Delete</Button>
                </Button.Group>
              </Table.Td>
            </Table.Tr>)}
        </Table.Tbody>

        <Table.Caption>
          <Group justify="center">
            <Pagination total={2} />
          </Group>
        </Table.Caption>
      </Table>
    </>
  )
}