import { Button, Divider, Group, Modal, NumberInput, Select, TextInput } from "@mantine/core";
import { useForm } from "@mantine/form";
import axios from "axios";
import { useEffect } from "react";

export default function CreateAccountModal({opened, onClose, onAccountCreated, accountTypes}) {
  const form = useForm({
    mode: 'uncontrolled',
    initialValues: {
      accountId: '',
      name: '',
      accountTypeId: null
    }
  })

  const asyncSubmit = async (formData) => {
    const response = await axios.post(import.meta.env.VITE_API_CHART_OF_ACCOUNT, formData)
    console.log(response);
  }

  // FIXME: Handle invalid input
  const handleSubmit = async (values) => {
    await asyncSubmit(values);
    form.reset()
    onClose()
    onAccountCreated?.(values)
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create new Account" size="xl">
      <form onSubmit={form.onSubmit(handleSubmit)}>
        <TextInput
          required
          label="Account ID"
          placeholder="100000"
          key={form.key('accountId')}
          {...form.getInputProps('accountId')}
        />

        <TextInput
          required
          label="Account Name"
          placeholder="Cash, Cash on bank, etc..."
          mt="md"
          key={form.key('name')}
          {...form.getInputProps('name')}
        />

        <Select
          required
          label="Account Type"
          data={accountTypes}
          mt="md"
          key={form.key('accountTypeId')}
          {...form.getInputProps('accountTypeId')}
        />

        <Group justify="flex-end" mt="md">
          <Button type="submit" loading={form.submitting}>Save</Button>
        </Group>
      </form>
    </Modal>
  )
}