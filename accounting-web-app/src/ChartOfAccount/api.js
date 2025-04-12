import {useEffect, useState} from "react";
import axios from "axios";

export async function getAccountTypes() {
  const result = await axios.get('/api/ChartOfAccounts/AccountTypes');
  return result.data;
}

export async function createAccount(account) {
  const result = await axios.post(`/api/ChartOfAccounts`, account);
  return result.data;
}

export async function getAccounts() {
  const result = await axios.get('/api/ChartOfAccounts');
  return result.data;
}

export async function getAccount(id) {
  if (id === undefined) {
    throw new Error("Invalid account id");
  }
  const result = await axios.get(`/api/ChartOfAccounts/${id}`);
  return result.data;
}

export async function updateAccount(id, account) {
  const result = await axios.put(`/api/ChartOfAccounts/${id}`, account);
  return result.data;
}

export function useAccountTypes() {
  const [accountTypes, setAccountTypes] = useState([])
  useEffect(() => {
    getAccountTypes(accountTypes)
      .then(setAccountTypes)
  }, [])
  return accountTypes
}

export function useAccount(id, handlerFn) {
  const [account, setAccount] = useState(null);
  useEffect(() => {
    if (id != null) {
      getAccount(id)
        .then(e => {
          setAccount(e);
          handlerFn?.(e);
        })
    }
  }, [id]);
  return account;
}