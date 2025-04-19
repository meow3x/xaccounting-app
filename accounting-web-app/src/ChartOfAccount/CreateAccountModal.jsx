import { Modal } from "@mantine/core";
import { useForm } from "@mantine/form";
import { AccountForm, createCoaFormParams } from "src/ChartOfAccount/AccountForm.jsx";
import {createAccount, useCreateAccount} from "src/ChartOfAccount/api.js";
import {useQueryClient} from "@tanstack/react-query";

export default function CreateAccountModal({ opened, onClose, onAccountCreated, accountTypes }) {
  const form = useForm(createCoaFormParams())
  const mutation = useCreateAccount()
  const queryClient = useQueryClient()

  function handleSubmit(formData) {
    mutation.mutate(formData, {
      onSuccess: (data) => {
        queryClient.invalidateQueries({ queryKey: ['accounts' ]})

        form.reset()
        onClose()
        onAccountCreated?.(data)
      }
    })

    // createAccount(formData)
    //   .then((account) => {
    //     form.reset()
    //     onAccountCreated?.(account)
    //     onClose()
    //   })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create new Account" size="xl">
      <AccountForm formController={form} accountTypes={accountTypes} handleSubmit={handleSubmit} />
    </Modal>
  )
}