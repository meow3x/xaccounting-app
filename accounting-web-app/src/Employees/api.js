import createRestClient from "src/util/api/restClient.js";
import {useEffect, useState} from "react";

export const employeeRepository = createRestClient('/api/Employees')

export function useEmployee(id, handlerFn) {
  const [item, setItem] = useState(null);
  useEffect(() => {
    if (id != null) {
      employeeRepository.getById(id).then(item => {
        setItem(item);
        handlerFn?.(item)
      });
    }
  }, [id])
  return item
}