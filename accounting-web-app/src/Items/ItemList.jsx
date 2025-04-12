import {ActionIcon, Box, Button, Group} from "@mantine/core";
import {IconEdit, IconEye, IconFileImport, IconPlus, IconSettingsCog, IconTrash} from "@tabler/icons-react";
import {Peso, DataTableRowActions, DataTableWrapper} from "src/util/table/common";
import {useEffect, useState} from "react";
import {useDisclosure} from "@mantine/hooks";
import {getItems, useItemCategories, useUnitOfMeasurements} from "src/Items/api.js";
import CreateItemModal from "src/Items/CreateItemModal.jsx";
import UpdateItemModal from "src/Items/UpdateItemModal.jsx";
import {notifications} from "@mantine/notifications";
import {showSuccessNotification} from "src/util/notification/notifications.js";
// import axios from "axios";


export default function ItemList() {
  const [isCreateModalOpen, createModalFn ] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn ] = useDisclosure(false)
  const [rowSelected, setRowSelected] = useState(null)
  const [records, setRecords] = useState([])
  const unitOfMeasurements = useUnitOfMeasurements()
  const itemCategories = useItemCategories()

  const columns = [
    { accessor: 'code', title: 'Item code' },
    { accessor: 'name' },
    { accessor: 'uom.name', title: 'Measurement' },
    { accessor: 'unitPrice', render: (item) => Peso(item.unitPrice) },
    { accessor: 'unitCost', render: (item) => Peso(item.unitCost) },
    { accessor: 'category.name', title: 'Category' },
    { accessor: 'wholesale' },
    { accessor: 'reorder' },
    {
      accessor: 'actions',
      title: 'Actions',
      textAlign: 'right',
      width: '0%',
      render: (item) => <DataTableRowActions onEditClick={() => {
        setRowSelected(item.id)
        updateModalFn.open()
      }} />
    }
  ]

  useEffect(() => {
    getItems().then(setRecords)
  }, [])

  return (
    <>
      <CreateItemModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        onItemCreated={(item) => {
          showSuccessNotification(`Item "${item.code} - ${item.name}" created`)
          getItems().then(setRecords)
        }}
        measurementsRef={unitOfMeasurements}
        categoriesRef={itemCategories} />

      <UpdateItemModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        measurementsRef={unitOfMeasurements}
        categoriesRef={itemCategories}
        itemId={rowSelected}
        onItemUpdated={(item) => {
          setRowSelected(null)
          showSuccessNotification(`Item "${item.code} - ${item.name}" updated`)
          getItems().then(setRecords)
        }}
      />

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

      <DataTableWrapper columns={columns} records={records} />
    </>
  )
}
