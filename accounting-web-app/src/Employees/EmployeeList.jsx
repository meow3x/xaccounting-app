import {useDisclosure} from "@mantine/hooks";
import {useEffect, useState} from "react";
import {DataTableRowActions, DataTableWrapper, Opt, Peso} from "src/util/table/common.jsx";
import {employeeRepository} from "src/Employees/api.js";
import CrudToolbar from "src/Shared/CrudToolbar.jsx";
import CreateEmployeeModal from "src/Employees/CreateEmployeeModal.jsx";
import {showSuccessNotification} from "src/util/notification/notifications.js";
import UpdateEmployeeModal from "src/Employees/UpdateEmployeeModal.jsx";

const SALARY_UNITS  = [
  "Hourly",
  "Weekly",
  "Monthly",
  "Annual"
]

export default function EmployeeList() {
  const [isCreateModalOpen, createModalFn ] = useDisclosure(false)
  const [isUpdateModalOpen, updateModalFn ] = useDisclosure(false)
  const [rowSelected, setRowSelected] = useState(null)
  const [records, setRecords] = useState([])
  const columns = [
    { accessor: 'employeeId' },
    { accessor: 'fullName', render: (row) => `${row.lastName}, ${row.firstName} ${Opt(row.middleName)}` },
    { accessor: 'address.street', title: 'Street' },
    { accessor: 'address.city', title: 'City' },
    { accessor: 'address.province', title: 'Province' },
    { accessor: 'address.landlineNumber', title: 'Landline' },
    { accessor: 'address.mobileNumber', title: 'Mobile' },
    { accessor: 'tin' },
    { accessor: 'pagIbigId', title: 'Pag-ibig Id' },
    { accessor: 'philhealthId', title: 'Philhealth ID' },
    { accessor: 'rate', render: item => Peso(item.rate) },
    { accessor: 'salaryUnit' },
    {
      accessor: 'actions',
      title: 'Actions',
      textAlign: 'right',
      width: '0%',
      render: (item) => <DataTableRowActions onEditClick={() => {
        setRowSelected(item.id)
        updateModalFn.open()
      }}/>
    }
  ]

  useEffect(() => {
    employeeRepository.getAll().then(setRecords)
  }, [])

  return (
    <>
      <CreateEmployeeModal
        opened={isCreateModalOpen}
        onClose={createModalFn.close}
        salaryUnitsRef={SALARY_UNITS}
        onDataCreated={(e => {
          showSuccessNotification(`Employee ${e.employeeId} created.`)
          employeeRepository.getAll().then(setRecords)
        })} />

      <UpdateEmployeeModal
        opened={isUpdateModalOpen}
        onClose={updateModalFn.close}
        salaryUnitsRef={SALARY_UNITS}
        recordId={rowSelected}
        onRecordUpdated={e => {
          setRowSelected(null)
          showSuccessNotification(`Employee ${e.employeeId} updated.`)
          employeeRepository.getAll().then(setRecords)
        }} />

      <CrudToolbar onCreate={createModalFn.open} />
      <DataTableWrapper columns={columns} records={records} />
    </>
  )
}