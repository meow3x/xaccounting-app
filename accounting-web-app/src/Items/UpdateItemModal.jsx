import {ItemForm, makeItemFormParams} from "src/Items/ItemForm.jsx";
import {updateItem, useItem, useItemCategories, useUnitOfMeasurements} from "src/Items/api.js";
import {useForm} from "@mantine/form";
import {Modal} from "@mantine/core";

export default function UpdateItemModal({ opened, onClose, onItemUpdated, itemId, measurementsRef, categoriesRef}) {
  const form = useForm(makeItemFormParams())

  useItem(itemId, (item) => {
    const values = {
      code: item.code,
      name: item.name,
      unitOfMeasurementId: item.uom.id.toString(),
      unitPrice: item.unitPrice,
      unitCost: item.unitCost,
      categoryId: item.category.id.toString(),
    }

    form.setValues(values)
    form.resetDirty(values)
  })

  function handleSubmit(formData) {
    updateItem(itemId, formData)
      .then(item => {
        form.reset()
        onClose()
        onItemUpdated(item)
      })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Edit Item" size="xl">
      <ItemForm
        formController={form}
        handleSubmit={handleSubmit}
        unitOfMeasurements={measurementsRef}
        itemCategories={categoriesRef}/>
    </Modal>
  )
}