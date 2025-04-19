import {useEffect, useState} from "react";
import axios from "axios";
import createBasicClient, {isNonEmpty, defaultGetAll, toQuerySyntax} from "src/util/api/restClient.js";
import {useMutation, useQuery} from "@tanstack/react-query";
import {PAGE_SIZES} from "src/util/table/common.jsx";

// const restClient = createBasicClient('/api/ChartOfAccounts');

export async function getAccountTypes() {
  const result = await axios.get('/api/ChartOfAccounts/AccountTypes');
  return result.data;
}

export async function createAccount(account) {
  const result = await axios.post(`/api/ChartOfAccounts`, account);
  return result.data;
}

export async function getAccounts(options = {}) {
  return defaultGetAll('/api/ChartOfAccounts', options)
}

export async function getAccount(id) {
  if (id === undefined) {
    throw new Error("Invalid account id");
  }
  const result = await axios.get(`/api/ChartOfAccounts/${id}`);
  return result.data;
}

export async function updateAccount({id, account}) {
  const result = await axios.put(`/api/ChartOfAccounts/${id}`, account);
  return result.data;
}

export function useAccountTypes() {
  return useQuery({
    queryKey: ['accountTypes'],
    queryFn: getAccountTypes,
    staleTime: 1000 * 60 * 5
  })
  // const [accountTypes, setAccountTypes] = useState([])
  // useEffect(() => {
  //   getAccountTypes(accountTypes)
  //     .then(setAccountTypes)
  // }, [])
  // return accountTypes
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

// wrap useQuery
export function useGetAccounts(options = {}) {
  return useQuery({
    queryKey: ['accounts', options],
    queryFn: () => getAccounts(options),
    staleTime: 60 * 1000 * 5
  })
}

export function useCreateAccount() {
  return useMutation({
    mutationFn: (e) => createAccount(e),
  })
}

export function useUpdateAccount() {
  return useMutation({
    mutationFn: (e) => updateAccount(e),
  })
}