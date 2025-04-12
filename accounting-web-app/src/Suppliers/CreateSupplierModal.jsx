import {useForm} from "@mantine/form";
import {Modal} from "@mantine/core";
import {makeSupplierForm, SupplierForm} from "src/Suppliers/SupplierForm.jsx";
import {createSupplier} from "src/Suppliers/api.js";

export default function CreateSupplierModal({ opened, onClose, onDataCreated, paymentTermRef }) {
  const form = useForm(makeSupplierForm())

  function handleSubmit(formData) {
    createSupplier(formData).then(e => {
      form.reset()
      onDataCreated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create supplier" size="xl">
      <SupplierForm
        formController={form}
        handleSubmit={handleSubmit}
        paymentTerms={paymentTermRef} />
    </Modal>
  )
}