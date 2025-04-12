import { Modal } from "@mantine/core";
import { useForm } from "@mantine/form";
import { AccountForm, makeChartFormParams } from "src/ChartOfAccount/AccountForm.jsx";
import { createAccount,  useAccountTypes } from "src/ChartOfAccount/api.js";

export default function CreateAccountModal({ opened, onClose, onAccountCreated }) {
  const accountTypes = useAccountTypes();
  const form = useForm(makeChartFormParams())

  function handleSubmit(formData) {
    createAccount(formData)
      .then((account) => {
        form.reset()
        onAccountCreated?.(account)
        onClose()
      })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create new Account" size="xl">
      <AccountForm formController={form} accountTypes={accountTypes} handleSubmit={handleSubmit} />
    </Modal>
  )
}