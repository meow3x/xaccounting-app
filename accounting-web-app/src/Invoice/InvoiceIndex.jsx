import { Modal } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import CrudToolbar from "src/Shared/CrudToolbar";
import InvoiceForm from "./InvoiceForm";

export default function InvoiceIndex() {
  const [invoiceModalOpened, invoiceModalFn] = useDisclosure(false)

  return (
    <>
      {/* <Modal opened={invoiceModalOpened} onClose={invoiceModalFn.close} title="Sales" size="80%"
        closeOnClickOutside={false}
        closeOnEscape={false}
        overlayProps={{
          backgroundOpacity: 0.55,
          blur: 3
        }}>
        <InvoiceForm onClose={invoiceModalFn.close} />
      </Modal> */}


      <CrudToolbar title="Invoice - Trade" />

      <InvoiceForm />
    </>
  )
}