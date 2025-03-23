import { Button, Divider, Group, Table, Text, Title , ActionIcon, Pagination} from "@mantine/core"
import { notifications } from "@mantine/notifications";
import axios from "axios";
import { useEffect, useState } from "react"
import { IconCross, IconEdit, IconPlus, IconTrash, IconX } from "@tabler/icons-react"
import { useDisclosure } from "@mantine/hooks"
import CreateAccountModal from "./CreateAccountModal"
import UpdateAccountModal from "./UpdateAccountModal";

export default function CoaIndex() {
  const headers = [ "Account ID", "Account Name", "Account Type", "Debit", "Credit", "Balance", "Year End Budget", "Controls"]
  const [accounts, setAccounts] = useState([])
  const [opened, { open, close }] = useDisclosure(false)
  const [accountAffected, setAccountAffected] = useState(false)
  // When updaing
  const [accountContext, setAccountContext] = useState(0)
  const [updateModalOpened, updateModalHandler] = useDisclosure(false)
  // Used in forms
  const [accountTypes, setAccountTypes] = useState({})

  const loadChart = () => {
    return axios.get(import.meta.env.VITE_API_CHART_OF_ACCOUNT).then(response => {
      setAccounts(response.data)
      console.log(response.data)
    })
  }

  const loadAccountTypes = () => {
    return axios.get(import.meta.env.VITE_API_ACCOUNT_TYPES).then(response => {
      setAccountTypes(
        response.data.map(e => ({
          value: e.id.toString(),
          label: e.label
        })
      ));
    })
  }

  useEffect(() => {
    loadChart()
  }, [])

  useEffect(() => {
    loadAccountTypes()
  }, [])

  return (
    <>
      <CreateAccountModal
        opened={opened}
        onClose={close}
        onAccountCreated={loadChart}
        accountTypes={accountTypes} />

      <UpdateAccountModal
        opened={updateModalOpened}
        onClose={updateModalHandler.close}
        accountTypes={accountTypes}
        onAccountUpdated={() => {
          console.log('affected');
          setAccountContext(null);
          loadChart();
        }}
        accountIdContext={accountContext}/>

      <Group justify="right">
        <Button onClick={open} rightSection={<IconPlus size={14} />}>Create Account</Button>
      </Group>

      <Table highlightOnHover withColumnBorders withRowBorders={true} striped captionSide="bottom">
        <Table.Thead>
          <Table.Tr>
            {headers.map(e => <Table.Th key={e}>{e}</Table.Th>)}
          </Table.Tr>
        </Table.Thead>

        <Table.Tbody>
          {accounts.map(e =>
            <Table.Tr key={e.id}>
              <Table.Td>{e.accountId}</Table.Td>
              <Table.Td>{e.name}</Table.Td>
              <Table.Td>{e.accountType.label}</Table.Td>
              <Table.Td>{e.debit}</Table.Td>
              <Table.Td>{e.credit}</Table.Td>
              <Table.Td>{0}</Table.Td>
              <Table.Td>{e.endBudget}</Table.Td>
              <Table.Td>
                <Button.Group>
                  <Button
                    size="sm"
                    variant="light"
                    leftSection={<IconEdit size={14}/>}
                    onClick={() => {
                      setAccountContext(e.id);
                      updateModalHandler.open()
                    }}>
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