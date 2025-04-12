import {useEffect, useState} from "react";
import axios from "axios";
import {getUnitOfMeasurements} from "src/Items/api.js";

// export const restFactory = createRestFactory({endpoint: '/api/Customers'});

export async function getCustomers() {
  const result = await axios.get('/api/Customers');
  return result.data;
}

export async function getCustomer(id) {
  if (id == null) {
    throw new Error('Invalid id');
  }
  const result = await axios.get(`/api/Customers/${id}`);
  return result.data;
}

export async function getPaymentTerms() {
  const result = await axios.get(`/api/Customers/PaymentTerms`);
  return result.data;
}

export async function createCustomer(customer) {
  const result = await axios.post('/api/Customers', customer);
  return result.data;
}

export async function updateCustomer(id, customer) {
  const result = await axios.put(`/api/Customers/${id}`, customer);
  return result.data
}

export function useCustomer(id, handlerFn) {
  const [item, setItem] = useState(null);
  useEffect(() => {
    if (id != null) {
      getCustomer(id).then(item => {
        setItem(item);
        handlerFn?.(item)
      });
    }
  }, [id])
  return item
}

export function usePaymentTerms() {
  const [paymentTerms, setPaymentTerms] = useState([]);
  useEffect(() => {
    getPaymentTerms().then(setPaymentTerms);
  }, [])
  return paymentTerms;
}
