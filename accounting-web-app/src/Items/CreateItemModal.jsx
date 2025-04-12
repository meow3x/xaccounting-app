import {createItem, useItemCategories, useUnitOfMeasurements} from "src/Items/api.js";
import {Modal} from "@mantine/core";
import {ItemForm, makeItemFormParams} from "src/Items/ItemForm.jsx";
import {useForm} from "@mantine/form";

export default function CreateItemModal({ opened, onClose, onItemCreated, measurementsRef, categoriesRef }) {
  const form = useForm(makeItemFormParams())

  function handleSubmit(formData) {
    createItem(formData).then(item => {
      form.reset()
      onItemCreated?.(item)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create Item" size="xl">
      <ItemForm
        formController={form}
        handleSubmit={handleSubmit}
        unitOfMeasurements={measurementsRef}
        itemCategories={categoriesRef} />
    </Modal>
  )
}