import {
  Button,
  Group,
  Select,
  TextInput
} from "@mantine/core";

export function AccountForm({ formController, handleSubmit, accountTypes }) {
  return (
    <form onSubmit={formController.onSubmit(handleSubmit)}>
      <TextInput
        required
        label="Account ID"
        placeholder="100000"
        key={formController.key('accountId')}
        {...formController.getInputProps('accountId')} />

      <TextInput
        required
        label="Account Name"
        placeholder="Cash, Cash on bank, etc..."
        mt="md"
        key={formController.key('name')}
        {...formController.getInputProps('name')} />

      <Select
        required
        label="Account Type"
        searchable
        checkIconPosition="right"
        data={accountTypes.map(e => ({
          value: e.id.toString(),
          label: e.label
        }))}
        mt="md"
        key={formController.key('accountTypeId')}
        {...formController.getInputProps('accountTypeId')} />

      <Group justify="flex-end" mt="md">
        <Button type="submit" loading={formController.submitting}>Save</Button>
      </Group>
    </form>
  )
}

export function makeChartFormParams() {
  return {
    mode: 'uncontrolled',
    initialValues: {
      accountId: '',
      name: '',
      accountTypeId: null
    }
  }
}