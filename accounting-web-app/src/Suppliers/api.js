import {useEffect, useState} from "react";
import axios from "axios";

// export const restFactory = createRestFactory({endpoint: '/api/Suppliers'});

export async function getSuppliers() {
  const result = await axios.get('/api/Suppliers');
  return result.data;
}

export async function getSupplier(id) {
  if (id == null) {
    throw new Error('Invalid id');
  }
  const result = await axios.get(`/api/Suppliers/${id}`);
  return result.data;
}

export async function getPaymentTerms() {
  const result = await axios.get(`/api/Customers/PaymentTerms`);
  return result.data;
}

export async function createSupplier(supplier) {
  const result = await axios.post('/api/Suppliers', supplier);
  return result.data;
}

export async function updateSupplier(id, supplier) {
  const result = await axios.put(`/api/Suppliers/${id}`, supplier);
  return result.data
}

export function useSupplier(id, handlerFn) {
  const [item, setItem] = useState(null);
  useEffect(() => {
    if (id != null) {
      getSupplier(id).then(item => {
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
