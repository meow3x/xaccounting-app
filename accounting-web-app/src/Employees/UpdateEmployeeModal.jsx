import {useForm} from "@mantine/form";
import {EmployeeForm, makeEmployeeForm} from "src/Employees/EmployeeForm.jsx";
import {employeeRepository, useEmployee} from "src/Employees/api.js";
import {Modal} from "@mantine/core";

export default function UpdateEmployeeModal({ opened, onClose, recordId, onRecordUpdated, salaryUnitsRef }) {
  const form = useForm(makeEmployeeForm())

  useEmployee(recordId, (employee) => {
    const values = {
      employeeId: employee.employeeId,
      firstName: employee.firstName,
      middleName: employee.middleName,
      lastName: employee.lastName,
      address: {
        street: employee.address.street,
        city: employee.address.city,
        province: employee.address.province,
        landlineNumber: employee.address.landlineNumber,
        mobileNumber: employee.address.mobileNumber,
      },
      tin: employee.tin,
      pagIbigId: employee.pagIbigId,
      philhealthId: employee.philhealthId,
      rate: employee.rate,
      salaryUnit: employee.salaryUnit
    }

    form.setValues(values)
    form.resetDirty(values);
  })

  function handleSubmit(formData) {
    employeeRepository.update(recordId, formData).then(e => {
      form.reset()
      onRecordUpdated?.(e)
      onClose()
    })
  }

  return (
    <Modal opened={opened} onClose={onClose} title="Update Employee" size="xl">
      <EmployeeForm
        formController={form}
        handleSubmit={handleSubmit}
        salaryUnits={salaryUnitsRef} />
    </Modal>
  )
}