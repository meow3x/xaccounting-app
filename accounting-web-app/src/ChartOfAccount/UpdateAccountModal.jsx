import { Modal } from "@mantine/core";
import {updateAccount, useAccount, useUpdateAccount} from "src/ChartOfAccount/api.js";
import { AccountForm, createCoaFormParams } from "src/ChartOfAccount/AccountForm.jsx";
import {useForm} from "@mantine/form";
import {useQueryClient} from "@tanstack/react-query";

export default function UpdateAccountModal({ opened, onClose, accountTypes, onAccountUpdated, accountId}) {
  const form = useForm(createCoaFormParams());
  const mutation = useUpdateAccount()
  const queryClient = useQueryClient()

  useAccount(accountId, (account) => {
    const values = {
      accountId: account.accountId,
      name: account.name,
      accountTypeId: account.accountType.id.toString(),
    };
    form.setValues(values);
    form.resetDirty(values);
  })

  function handleSubmit (formData)  {
    mutation.mutate({
      id: accountId,
      account: formData
    }, {
      onSuccess: (data) => {
        queryClient.invalidateQueries({ queryKey: ['accounts'] })

        form.reset()
        onClose()
        onAccountUpdated?.(data)
      }
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Update Account" size="xl">
      <AccountForm formController={form} accountTypes={accountTypes} handleSubmit={handleSubmit} />
    </Modal>
  )
}