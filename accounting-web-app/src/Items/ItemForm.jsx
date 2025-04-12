import {
  Button,
  Group,
  NumberInput,
  Select,
  TextInput
} from "@mantine/core";

export function makeItemFormParams() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      code: '',
      name: '',
      unitOfMeasurementId: null,
      unitPrice: 0.0,
      unitCost: 0.0,
      categoryId: null
    }
  }
}

export function ItemForm({ formController, handleSubmit, unitOfMeasurements, itemCategories }) {
  return (
    <form onSubmit={formController.onSubmit(handleSubmit)}>
      <TextInput
        required
        label="Item code"
        placeholer="Item code"
        key={formController.key('code')}
        {...formController.getInputProps('code') } />

      <TextInput
        required
        label="Name"
        placeholer="Item name"
        mt="md"
        key={formController.key('name')}
        {...formController.getInputProps('name')} />

      <Group justify="space-between" mt="md" grow>
        <Select
          required
          label="Unit of measurement"
          searchable
          checkIconPosition="right"
          data={unitOfMeasurements.map(e => ({
            value: e.id.toString(),
            label: e.name
          }))}
          key={formController.key('unitOfMeasurementId')}
          {...formController.getInputProps('unitOfMeasurementId')} />

        <NumberInput
          required
          label="Unit Price"
          placeholer="Item unit price"
          key={formController.key('unitPrice')}
          {...formController.getInputProps('unitPrice')} />

        <NumberInput
          required
          label="Unit cost"
          placeholer="Item unit cost"
          key={formController.key('unitCost')}
          {...formController.getInputProps('unitCost')} />
      </Group>

      <Select
        required
        label="Category"
        mt="md"
        searchable
        checkIconPosition="right"
        data={itemCategories.map(e => ({
          value: e.id.toString(),
          label: e.name
        }))}
        key={formController.key('categoryId')}
        {...formController.getInputProps('categoryId')} />

      <Group justify="flex-end" mt="md">
        <Button type="submit" loading={formController.submitting}>Save</Button>
      </Group>
    </form>
  )
}