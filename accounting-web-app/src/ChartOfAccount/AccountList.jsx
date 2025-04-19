import {ActionIcon, Button, Group, MultiSelect, TextInput} from "@mantine/core"
import { useEffect, useState } from "react"
import {
  IconFileImport,
  IconPlus, IconSearch, IconX
} from "@tabler/icons-react"
import {useDebouncedValue, useDisclosure} from "@mantine/hooks"
import CreateAccountModal from "./CreateAccountModal.jsx"
import UpdateAccountModal from "./UpdateAccountModal.jsx";
import {
  Peso,
  DataTableRowActions,
  DataTableWrapper, usePaginationState
} from "src/util/table/common";
import {getAccounts, useAccountTypes, useGetAccounts} from "src/ChartOfAccount/api";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import {MultiSelectFilter, TextFilter} from "src/Shared/Filters.jsx";

export default function AccountList() {
  const [isCreateModalOpen, createModalFn] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn] = useDisclosure(false)
  
  const [targetEdit, setTargetEdit] = useState(null)

  const [records, setRecords] = useState([])
  const [totalRecords, setTotalRecords] = useState(0)
  const accountTypes = useAccountTypes()

  // filters
  // const [isFetching, setIsFetching] = useState(false)
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
      accessor: 'accountType.label',
      title: 'Account Type',
      filter: (
        <MultiSelectFilter
          label="Account type"
          data={accountTypes.map(e => ({
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

  /*useEffect(() => {
    console.log('state', {
      page: paginationState.page,
      pageSize: paginationState.recordsPerPage,
      name: debouncedName,
      selectedAccountTypes: selectedAccountTypes
    })
    setIsFetching(true)

    getAccounts({
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
    }).then(e => {
      setTotalRecords(e._meta.total)
      setRecords(e.data)
    }).finally(() => {
      setIsFetching(false)
    })
  // Add recordsPerPage to allow refetch if page size was changed on current page
  }, [
    paginationState.page,
    paginationState.recordsPerPage,
    // Filters
    debouncedName,
    selectedAccountTypes,
  ])*/

  return (
    <>
      <CreateAccountModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        accountTypes={accountTypes}
        onAccountCreated={(account) => {
          showSuccessNotification(`Account ${account.accountId} - ${account.name} created.`)
          // getAccounts().then(setRecords)
        }}/>

      <UpdateAccountModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        accountId={targetEdit}
        accountTypes={accountTypes}
        onAccountUpdated={(account) => {
          setTargetEdit(null);
          showSuccessNotification(`Account ${account.accountId} - ${account.name} updated.`)
          // getAccounts().then(setRecords)
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

      <DataTableWrapper
        columns={columns}
        records={data?.records}
        totalRecords={data?._meta.total}
        paginationState={paginationState}
        isFetching={isFetching} />
    </>
  )
}