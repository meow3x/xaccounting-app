import axios from "axios";
import {useEffect, useState} from "react";

export async function getItemCategories() {
  const result = await axios.get('/api/Items/Categories');
  return result.data;
}

export async function getUnitOfMeasurements() {
  const result = await axios.get('/api/Items/UnitOfMeasurements');
  return result.data;
}

export async function getItems() {
  const result = await axios.get('/api/Items');
  return result.data;
}

export async function getItem(id) {
  if (id == null) {
    throw new Error('Invalid id');
  }
  const result = await axios.get(`/api/Items/${id}`);
  return result.data;
}

export async function createItem(item) {
  const result = await axios.post('/api/Items', item);
  return result.data;
}

export async function updateItem(id, item) {
  const result = await axios.put(`/api/Items/${id}`, item);
  return result.data
}

export function useItemCategories() {
  const [categories, setCategories] = useState([]);
  useEffect(() => {
    getItemCategories().then(setCategories);
  }, [])
  return categories;
}

export function useUnitOfMeasurements() {
  const [unitOfMeasurements, setUnitOfMeasurements] = useState([]);
  useEffect(() => {
    getUnitOfMeasurements().then(setUnitOfMeasurements);
  }, [])
  return unitOfMeasurements;
}

export function useItem(id, handlerFn) {
  const [item, setItem] = useState(null);
  useEffect(() => {
    if (id != null) {
      getItem(id).then(item => {
        setItem(item);
        handlerFn?.(item)
      });
    }
  }, [id])
  return item
}