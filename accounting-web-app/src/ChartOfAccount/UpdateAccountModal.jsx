import { Button, Group, Modal, Select, TextInput} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useEffect } from "react";
import axios from "axios";

export default function UpdateAccountModal({opened, onClose, onAccountUpdated, accountIdContext, accountTypes}) {
  const form = useForm({
    mode: 'uncontrolled',
    initialValues: {
      accountId: '',
      name: "",
      accountTypeId: null
    }
  })

  // Get account data
  useEffect(() => {
    if (accountIdContext) {
      axios.get(import.meta.env.VITE_API_CHART_OF_ACCOUNT + "/" + accountIdContext)
      .then((response) => {
        console.log(response)
        form.setValues({
          accountId: response.data.accountId,
          name: response.data.name,
          accountTypeId: response.data.accountType.id.toString()
        })
        form.resetDirty()
      })
    }
  }, [accountIdContext])

  const asyncSubmit = async (form ) => {
    const response = await axios.put(import.meta.env.VITE_API_CHART_OF_ACCOUNT + "/" + accountIdContext, {
      id: accountIdContext,
      ...form
    })
    console.log(response)
  }
  var handleSubmit = async (values) => {
    await asyncSubmit(values)
    form.reset()
    onClose()
    onAccountUpdated()
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Update Account" size="xl">
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