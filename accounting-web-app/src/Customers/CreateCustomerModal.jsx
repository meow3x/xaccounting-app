import {useForm} from "@mantine/form";
import {CustomerForm, makeCustomerForm} from "src/Customers/CustomerForm.jsx";
import {Modal} from "@mantine/core";
import {createCustomer} from "src/Customers/api.js";

export default function CreateCustomerModal({ opened, onClose, onCustomerCreated, paymentTermRef }) {
  const form = useForm(makeCustomerForm())

  function handleSubmit(formData) {
    createCustomer(formData).then(e => {
      form.reset()
      onCustomerCreated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Create customer" size="xl">
      <CustomerForm
        formController={form}
        handleSubmit={handleSubmit}
        paymentTerms={paymentTermRef} />
    </Modal>
  )
}