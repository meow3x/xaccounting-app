import {ActionIcon, Button, Group, MultiSelect, Text, TextInput, Title} from "@mantine/core"
import { useEffect, useState } from "react"
import {
  IconCirclePlus,
  IconFileImport,
  IconPlus, IconSearch, IconUpload, IconX
} from "@tabler/icons-react"
import {useDebouncedValue, useDisclosure} from "@mantine/hooks"
import CreateAccountModal from "./CreateAccountModal.jsx"
import UpdateAccountModal from "./UpdateAccountModal.jsx";
import {
  Peso,
  DataTableRowActions,
  DataTableWrapper
} from "src/util/table/common";
import {getAccounts, useAccountTypes, useGetAccounts} from "src/ChartOfAccount/api";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import {MultiSelectFilter, TextFilter} from "src/Shared/Filters.jsx";
import {usePaginationState} from "src/util/table/pagination.jsx";
import CrudToolbar from "src/Shared/CrudToolbar.jsx";

export default function AccountList() {
  const [isCreateModalOpen, createModalFn] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn] = useDisclosure(false)
  const [targetEdit, setTargetEdit] = useState(null)
  const { data: accountTypes } = useAccountTypes()

  // filters
  const [idQuery, setIdQuery] = useState('')
  const [debouncedId] = useDebouncedValue(idQuery, 300)
  const [nameQuery, setNameQuery] = useState('')
  const [debouncedName] = useDebouncedValue(nameQuery, 500)
  const [selectedAccountTypes, setSelectedAccountTypes] = useState([])

  const columns = [
    {
      accessor: 'accountId',
      filter: <TextFilter label="Account ID" query={idQuery} setQuery={setIdQuery} />,
      // filtering: idQuery != ''
    },
    {
      accessor: 'name',
      filter: <TextFilter label="Name" query={nameQuery} setQuery={setNameQuery} />
    },
    {
      accessor: 'accountType.name',
      title: 'Account Type',
      filter: (
        <MultiSelectFilter
          label="Account type"
          data={accountTypes?.map(e => ({
            value: e.id.toString(),
            label: e.label
          }))}
          value={selectedAccountTypes}
          onChange={setSelectedAccountTypes} />
      ),
      filtering: selectedAccountTypes.length > 0
    },
    { accessor: 'debit', render: (account) => Peso(account.debit) },
    { accessor: 'credit',render: (account) => Peso(account.credit) },
    { accessor: 'endBudget', title: 'Year-end budget', render: (account) => Peso(account.endBudget) },
    {
      accessor: 'actions',
      title: 'Actions',
      textAlign: 'right',
      width: '0%',
      render: (item) => <DataTableRowActions onEditClick={() => {
        setTargetEdit(item.id)
        updateModalFn.open()
      }} />
    }
  ]

  // Pagination
  const paginationState = usePaginationState([10,20,50,100])

  const { data, isFetching } = useGetAccounts({
    pagination: {
      page: paginationState.page,
      pageSize: paginationState.recordsPerPage,
    },
    filters: [
      {
        field: 'name',
        value: debouncedName
      },
      {
        field: 'account_type',
        value: selectedAccountTypes.map(e => e)
      }
    ]
  })

  return (
    <>
      <CreateAccountModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        accountTypes={accountTypes}
        onAccountCreated={(account) => {
          showSuccessNotification(`Account ${account.accountId} - ${account.name} created.`)
        }}/>

      <UpdateAccountModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        accountId={targetEdit}
        accountTypes={accountTypes}
        onAccountUpdated={(account) => {
          setTargetEdit(null);
          showSuccessNotification(`Account ${account.accountId} - ${account.name} updated.`)
        }} />

      <CrudToolbar title="Chart Of Accounts" onCreate={createModalFn.open} />

      <DataTableWrapper
        columns={columns}
        records={data?.records}
        totalRecords={data?._meta.total}
        paginationState={paginationState}
        isFetching={isFetching} />
    </>
  )
}