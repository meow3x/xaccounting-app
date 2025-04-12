import { Button, Group } from "@mantine/core"
import { useEffect, useState } from "react"
import {
  IconFileImport,
  IconPlus
} from "@tabler/icons-react"
import { useDisclosure } from "@mantine/hooks"
import CreateAccountModal from "./CreateAccountModal.jsx"
import UpdateAccountModal from "./UpdateAccountModal.jsx";
import {
  Peso,
  DataTableRowActions,
  DataTableWrapper
} from "src/util/table/common";
import { getAccounts } from "src/ChartOfAccount/api";
import {showSuccessNotification} from "src/util/notification/notifications.js";

export default function AccountList() {
  const [isCreateModalOpen, createModalFn] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn] = useDisclosure(false)
  const [accountSelected, setAccountSelected] = useState(null)
  const [records, setRecords] = useState([])
  const columns = [
    { accessor: 'accountId' },
    { accessor: 'name'},
    { accessor: 'accountType.label', title: 'Account Type' },
    { accessor: 'debit', render: (account) => Peso(account.debit) },
    { accessor: 'credit',render: (account) => Peso(account.credit) },
    { accessor: 'endBudget', title: 'Year-end budget', render: (account) => Peso(account.endBudget) },
    {
      accessor: 'actions',
      title: 'Actions',
      textAlign: 'right',
      width: '0%',
      render: (item) => <DataTableRowActions onEditClick={() => {
        setAccountSelected(item.id)
        updateModalFn.open()
      }} />
    }
  ]

  useEffect(() => {
    getAccounts().then(setRecords);
  }, [])

  return (
    <>
      <CreateAccountModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        onAccountCreated={(account) => {
          showSuccessNotification(`Account ${account.accountId} - ${account.name} created.`)
          getAccounts().then(setRecords)
        }}/>

      <UpdateAccountModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        accountId={accountSelected}
        onAccountUpdated={(account) => {
          setAccountSelected(null);
          showSuccessNotification(`Account ${account.accountId} - ${account.name} updated.`)
          getAccounts().then(setRecords)
        }} />

      <Group justify="left" gap={4} mb={30}>
        <Button
          variant="filled"
          rightSection={<IconPlus size={16} />}
          onClick={createModalFn.open}
        >
          New
        </Button>
        <Button variant="outline" rightSection={<IconFileImport size={16} />}>Import</Button>
      </Group>

      <DataTableWrapper columns={columns} records={records} />
    </>
  )
}