import {Peso, DataTableRowActions, DataTableWrapper, Percentage} from "src/util/table/common.jsx";
import {ActionIcon, Button, Group} from "@mantine/core";
import {IconEdit, IconEye, IconFileImport, IconPlus, IconTrash} from "@tabler/icons-react";
import CreateCustomerModal from "src/Customers/CreateCustomerModal.jsx";
import {useDisclosure} from "@mantine/hooks";
import {useEffect, useState} from "react";
import {getSuppliers, usePaymentTerms} from "src/Suppliers/api.js";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import CreateSupplierModal from "src/Suppliers/CreateSupplierModal.jsx";
import UpdateSupplierModal from "src/Suppliers/UpdateSupplierModal.jsx";

export default function SupplierList() {
  const [isCreateModalOpen, createModalFn ] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn ] = useDisclosure(false)
  const [rowSelected, setRowSelected] = useState(null)
  const [records, setRecords] = useState([])
  const paymentTerms = usePaymentTerms()

  const columns = [
    { accessor: 'supplierId' },
    { accessor: 'name' },
    { accessor: 'address.street', title: 'Street' },
    { accessor: 'address.city', title: 'City' },
    { accessor: 'address.province', title: 'Province' },
    { accessor: 'address.landlineNumber', title: 'Landline' },
    { accessor: 'address.mobileNumber', title: 'Mobile' },
    { accessor: 'tin' },
    { accessor: 'discount', render: (e) => Percentage(e.discount) },
    { accessor: 'creditLimit', render: (e) => Peso(e.creditLimit) },
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
    getSuppliers().then(setRecords)
  }, [])

  return (
    <>
      <CreateSupplierModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        paymentTermRef={paymentTerms}
        onDataCreated={e => {
          showSuccessNotification(`Supplier ${e.supplierId} - ${e.name} created.`)
          getSuppliers().then(setRecords)
        }}/>

      <UpdateSupplierModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        paymentTermRef={paymentTerms}
        recordId={rowSelected}
        onRecordUpdated={e => {
          setRowSelected(null)
          showSuccessNotification(`Supplier ${e.supplierId} - ${e.name} updated.`)
          getSuppliers().then(setRecords)
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