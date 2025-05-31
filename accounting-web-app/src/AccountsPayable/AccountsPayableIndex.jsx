import CrudToolbar from "src/Shared/CrudToolbar";
import AccountsPayableForm from "./AccountsPayableForm";
import { AccountsPayableList } from "./AccountsPayableList";
import { Modal, ScrollArea, Stack } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import DisbursementForm from "src/Disbursements/DisbursementForm";
import { useState } from "react";

export default function AccountsPayableIndex() {
  const [modalOpened, modalFn] = useDisclosure(false)
  const [disbursementModalOpened, disbursementFormModalFn] = useDisclosure(false)
  const [targetPayable, setTargetPayable] = useState(null)

  return (
    <>
      {/* New A/P form */}
      <Modal opened={modalOpened} onClose={modalFn.close} title="Non-Trade Payable (New)" size="80%"
        closeOnClickOutside={false}
        closeOnEscape={false}
        overlayProps={{
          backgroundOpacity: 0.55,
          blur: 3
        }}>
        <AccountsPayableForm onClose={modalFn.close} />
      </Modal>

      {/* New Payment form */}
      <Modal opened={disbursementModalOpened} onClose={disbursementFormModalFn.close} title="Create Payment" size="80%"
        closeOnClickOutside={false}
        closeOnEscape={false}
        overlayProps={{
          backgroundOpacity: 0.55,
          blur: 3
        }}>
        <DisbursementForm apVoucherNumber={targetPayable} onClose={disbursementFormModalFn.close} />
      </Modal>

      <CrudToolbar title="Accounts Payable (Non-Trade)" onCreate={modalFn.open} />

      <AccountsPayableList onPaymentRequest={(ap) => {
        setTargetPayable(ap.voucherNumber)
        disbursementFormModalFn.open()
      }} />
    </>
  )
}