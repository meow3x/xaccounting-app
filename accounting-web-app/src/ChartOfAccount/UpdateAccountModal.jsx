import { Modal} from "@mantine/core";
import { useEffect } from "react";
import {
  updateAccount,
  useAccount,
  useAccountTypes
} from "src/ChartOfAccount/api.js";
import { AccountForm, makeChartFormParams } from "src/ChartOfAccount/AccountForm.jsx";
import {useForm} from "@mantine/form";

export default function CreateAccountModal({ opened, onClose, onAccountUpdated, accountId}) {
  const form = useForm(makeChartFormParams());
  const accountTypes = useAccountTypes()

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
    updateAccount(accountId, formData)
      .then(account => {
        form.reset()
        onClose()
        onAccountUpdated(account)
      })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Update Account" size="xl">
      <AccountForm formController={form} accountTypes={accountTypes} handleSubmit={handleSubmit} />
    </Modal>
  )
}