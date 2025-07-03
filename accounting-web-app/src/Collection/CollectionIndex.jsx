import CrudToolbar from "src/Shared/CrudToolbar";
import CollectionCasesList from "./CollectionCasesList";
import { useState } from "react";
import { useDisclosure } from "@mantine/hooks";
import { Modal } from "@mantine/core";
import CollectionForm from "./CollectionForm";

export default function CollectionIndex() {
  const [targetInvoice, setTargetInvoice] = useState(null);
  const [colModalOpened, colModalFn] = useDisclosure(false)

  return (
    <>
      {/* Collection form */}
      <Modal opened={colModalOpened} onClose={colModalFn.close} title="Cash Collection" size="80%"
        closeOnClickOutside={false}
        closeOnEscape={false}
        overlayProps={{
          backgroundOpacity: 0.55,
          blur: 3
      }}>
        <CollectionForm invoiceNumber={targetInvoice} onClose={colModalFn.close}/>
      </Modal>

      <CrudToolbar title="Collections" />

      <CollectionCasesList onPaymentRequest={(invoice) => {
        setTargetInvoice(invoice.number)
        colModalFn.open()
      }} />
    </>
  )
}