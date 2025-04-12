import {useForm} from "@mantine/form";
import {CustomerForm, makeCustomerForm} from "src/Customers/CustomerForm.jsx";
import {updateCustomer, useCustomer} from "src/Customers/api.js";
import {Modal} from "@mantine/core";


export default function UpdateCustomerModal({ opened, onClose, customerId, onCustomerUpdated, paymentTermRef }) {
  const form = useForm(makeCustomerForm())

  useCustomer(customerId, (customer) => {
    const values = {
      customerId: customer.customerId,
      name: customer.name,
      address: {
        street: customer.address?.street,
        city: customer.address?.city,
        province: customer.address?.province,
        landlineNumber: customer.address?.landlineNumber,
        mobileNumber: customer.address?.mobileNumber,
      },
      tin: customer.tin,
      discount: customer.discount,
      creditLimit: customer.creditLimit,
      paymentTermId: customer.paymentTerm?.id.toString(),
    }

    form.setValues(values)
    form.resetDirty(values)
  })

  function handleSubmit(formData) {
    updateCustomer(customerId, formData).then(e => {
      form.reset()
      onCustomerUpdated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Edit customer" size="xl">
      <CustomerForm
        formController={form}
        handleSubmit={handleSubmit}
        paymentTerms={paymentTermRef} />
    </Modal>
  )
}