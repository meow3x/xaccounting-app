import {Button, Fieldset, Group, NumberInput, Select, TextInput} from "@mantine/core";

export function makeSupplierForm() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      supplierId: '',
      name: '',
      address: {
        street: '',
        city: '',
        province: '',
        landlineNumber: '',
        mobileNumber: ''
      },
      tin: '',
      discount: null,
      creditLimit: null,
      paymentTermId: null
    }
  }
}

export function SupplierForm({ formController, handleSubmit, paymentTerms }) {
  return (
    <form onSubmit={formController.onSubmit(handleSubmit)}>
      <TextInput
        required
        label="Supplier Id"
        key={formController.key('supplierId')}
        {...formController.getInputProps('supplierId')} />

      <TextInput
        required
        label="Supplier name"
        mt="md"
        key={formController.key('name')}
        {...formController.getInputProps('name')} />

      <Group mt="md" grow>
        <TextInput
          required
          label="Street"
          key={formController.key('address.street')}
          {...formController.getInputProps('address.street')} />

        <TextInput
          required
          label="City"
          key={formController.key('address.city')}
          {...formController.getInputProps('address.city')} />

        <TextInput
          required
          label="Province"
          key={formController.key('address.province')}
          {...formController.getInputProps('address.province')} />
      </Group>

      <Group mt="md" grow>
        <TextInput
          label="Landline number"
          key={formController.key('address.landlineNumber')}
          {...formController.getInputProps('address.landlineNumber')} />

        <TextInput
          label="Mobile number"
          key={formController.key('address.mobileNumber')}
          {...formController.getInputProps('address.mobileNumber')} />
      </Group>

      <TextInput
        label="Tin"
        mt="md"
        key={formController.key('tin')}
        {...formController.getInputProps('tin')} />

      <Group mt="md" grow>
        <NumberInput
          label="Discount"
          key={formController.key('discount')}
          {...formController.getInputProps('discount')} />

        <NumberInput
          label="Credit limit"
          key={formController.key('creditLimit')}
          {...formController.getInputProps('creditLimit')} />

        <Select
          label="Payment term"
          searchable
          checkIconPosition="right"
          data={paymentTerms.map(e => ({
            value: e.id.toString(),
            label: e.label
          }))}
          key={formController.key('paymentTermId')}
          {...formController.getInputProps('paymentTermId')} />
      </Group>

      <Group justify="flex-end" mt="md">
        <Button type="submit" loading={formController.submitting}>Save</Button>
      </Group>
    </form>
  )
}