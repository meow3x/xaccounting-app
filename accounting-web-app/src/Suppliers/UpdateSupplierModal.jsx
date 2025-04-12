import {useForm} from "@mantine/form";
import {Modal} from "@mantine/core";
import {makeSupplierForm, SupplierForm} from "src/Suppliers/SupplierForm.jsx";
import {updateSupplier, useSupplier} from "src/Suppliers/api.js";

export default function UpdateSupplierModal({ opened, onClose, recordId, onRecordUpdated, paymentTermRef }) {
  const form = useForm(makeSupplierForm())

  useSupplier(recordId, (supplier) => {
    const values = {
      supplierId: supplier.supplierId,
      name: supplier.name,
      address: {
        street: supplier.address.street,
        city: supplier.address.city,
        province: supplier.address.province,
        landlineNumber: supplier.address.landlineNumber,
        mobileNumber: supplier.address.mobileNumber,
      },
      tin: supplier.tin,
      discount: supplier.discount,
      creditLimit: supplier.creditLimit,
      paymentTermId: supplier.paymentTerm?.id.toString(),
    }

    form.setValues(values)
    form.resetDirty(values)
  })

  function handleSubmit(formData) {
    updateSupplier(recordId, formData).then(e => {
      form.reset()
      onRecordUpdated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Edit Supplier" size="xl">
      <SupplierForm
        formController={form}
        handleSubmit={handleSubmit}
        paymentTerms={paymentTermRef} />
    </Modal>
  )
}