import {Peso, DataTableRowActions, DataTableWrapper, Percentage} from "src/util/table/common.jsx";
import {ActionIcon, Button, Group} from "@mantine/core";
import {IconEdit, IconEye, IconFileImport, IconPlus, IconTrash} from "@tabler/icons-react";
import CreateCustomerModal from "src/Customers/CreateCustomerModal.jsx";
import {useDisclosure} from "@mantine/hooks";
import {useEffect, useState} from "react";
import {getCustomers, usePaymentTerms} from "src/Customers/api.js";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import UpdateCustomerModal from "src/Customers/UpdateCustomerModal.jsx";

export default function CustomerList() {
  const [isCreateModalOpen, createModalFn ] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn ] = useDisclosure(false)
  const [rowSelected, setRowSelected] = useState(null)
  const [records, setRecords] = useState([])
  const paymentTerms = usePaymentTerms()

  const columns = [
    { accessor: 'customerId' },
    { accessor: 'name' },
    { accessor: 'address.street', title: 'Street' },
    { accessor: 'address.city', title: 'City' },
    { accessor: 'address.province', title: 'Province' },
    { accessor: 'address.landlineNumber', title: 'Landline' },
    { accessor: 'address.mobileNumber', title: 'Mobile' },
    { accessor: 'tin' },
    { accessor: 'discount', render: (customer) => Percentage(customer.discount) },
    { accessor: 'creditLimit', render: (customer) => Peso(customer.creditLimit) },
    { accessor: 'paymentTerm.label', title: 'Payment Term' },
    {
      accessor: 'actions',
      title: 'Actions',
      textAlign: 'right',
      width: '0%',
      render: (item) => <DataTableRowActions onEditClick={() => {
        setRowSelected(item.id)
        updateModalFn.open()
      }}/>
    }
  ]

  useEffect(() => {
    getCustomers().then(setRecords)
  }, [])

  return (
    <>
      <CreateCustomerModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        title="Create customer"
        paymentTermRef={paymentTerms}
        onCustomerCreated={e => {
          showSuccessNotification(`Customer ${e.customerId} - ${e.name} created.`)
          getCustomers().then(setRecords)
        }}/>

      <UpdateCustomerModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        paymentTermRef={paymentTerms}
        customerId={rowSelected}
        onCustomerUpdated={e => {
          setRowSelected(null)
          showSuccessNotification(`Customer ${e.customerId} - ${e.name} updated.`)
          getCustomers().then(setRecords)
        }} />

      <Group justify="left" gap={4} mb={30}>
        <Button
          variant="filled"
          rightSection={<IconPlus size={16} />}
          onClick={createModalFn.open}
        >
          New
        </Button>
        <Button variant="outline" rightSection={<IconFileImport size={16} />}>
          Import
        </Button>
      </Group>

      <DataTableWrapper columns={columns} records={records}/>
    </>
  )
}