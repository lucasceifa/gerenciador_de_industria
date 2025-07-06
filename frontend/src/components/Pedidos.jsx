/* eslint-disable no-unused-vars */
import React, { useState, useEffect } from 'react';
import {
  Table,
  TableHead,
  TableHeadCell,
  TableBody,
  TableRow,
  TableCell,
  Button,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Select,
  TextInput,
  Label,
  Toast,
  Spinner
} from 'flowbite-react';
import { HiCheck, HiX, HiExclamation, HiPlus, HiTrash } from 'react-icons/hi';
import PedidoService from '../services/PedidoService';
import CarneService from '../services/CarneService';
import CompradorService from '../services/CompradorService';
import CurrencyService from '../services/CurrencyService';

export default function Pedidos() {
  const [pedidos, setPedidos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showFormModal, setShowFormModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  const [currentPedido, setCurrentPedido] = useState(null);
  const [carnes, setCarnes] = useState([]);
  const [compradores, setCompradores] = useState([]);
  const [currencyRates, setCurrencyRates] = useState({});

  const [toast, setToast] = useState({ show: false, message: '', type: 'success' });

  const initialItemState = { carneId: '', precoUnitario: '', moeda: 'BRL' };
  const [newItem, setNewItem] = useState(initialItemState);

  useEffect(() => {
    fetchInitialData();
  }, []);

  const fetchInitialData = async () => {
    setLoading(true);
    try {
      const [pedidosRes, carnesRes, compradoresRes, ratesRes] = await Promise.all([
        PedidoService.getAll(),
        CarneService.getAll(),
        CompradorService.getAll(),
        CurrencyService.getRates()
      ]);
      setPedidos(pedidosRes.data);
      setCarnes(carnesRes.data);
      setCompradores(compradoresRes.data);

      const rates = {
        'USD': parseFloat(ratesRes.data.USDBRL.ask),
        'EUR': parseFloat(ratesRes.data.EURBRL.ask),
        'BRL': 1
      };
      setCurrencyRates(rates);

    } catch (error) {
      showToastMessage('Erro ao carregar dados iniciais.', 'failure');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const fetchPedidos = async () => {
    setLoading(true);
    try {
      const response = await PedidoService.getAll();
      setPedidos(response.data);
    } catch (error) {
      showToastMessage('Erro ao buscar pedidos.', 'failure');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const showToastMessage = (message, type = 'success') => {
    setToast({ show: true, message, type });
    setTimeout(() => setToast({ show: false, message: '', type: 'success' }), 3000);
  };

  const calculateTotalBRL = (itens) => {
    if (!itens || !currencyRates.USD) return 0;
    return itens.reduce((total, item) => {
      const rate = currencyRates[item.moeda] || 1;
      return total + (item.precoUnitario * rate);
    }, 0);
  };

  const handleNewPedido = () => {
    setCurrentPedido({
      dataPedido: new Date().toISOString().split('T')[0],
      compradorId: '',
      itens: []
    });
    setShowFormModal(true);
  };

  const handleEdit = (pedido) => {
    const formattedPedido = {
      ...pedido,
      dataPedido: new Date(pedido.dataPedido).toISOString().split('T')[0]
    };
    setCurrentPedido(formattedPedido);
    setShowFormModal(true);
  };

  const handleDelete = (pedido) => {
    setCurrentPedido(pedido);
    setShowDeleteModal(true);
  };

  const handleAddItem = () => {
    if (!newItem.carneId || !newItem.precoUnitario || parseFloat(newItem.precoUnitario) <= 0) {
      showToastMessage('Selecione uma carne e um preço válido.', 'failure');
      return;
    }
    setCurrentPedido(prev => ({
      ...prev,
      itens: [...prev.itens, { ...newItem, precoUnitario: parseFloat(newItem.precoUnitario) }]
    }));
    setNewItem(initialItemState);
  };

  const handleRemoveItem = (index) => {
    setCurrentPedido(prev => ({
      ...prev,
      itens: prev.itens.filter((_, i) => i !== index)
    }));
  };

  const handleSave = async () => {
    if (!currentPedido.dataPedido || !currentPedido.compradorId || currentPedido.itens.length === 0) {
      showToastMessage('Data, comprador e ao menos um item são obrigatórios.', 'failure');
      return;
    }
    try {
      if (currentPedido.id) {
        await PedidoService.update(currentPedido.id, currentPedido);
        showToastMessage('Pedido atualizado com sucesso!');
      } else {
        await PedidoService.create(currentPedido);
        showToastMessage('Pedido criado com sucesso!');
      }
      setShowFormModal(false);
      fetchPedidos();
    } catch (error) {
      showToastMessage('Erro ao salvar o pedido.', 'failure');
      console.log(error);
    }
  };

  const confirmDelete = async () => {
    try {
      await PedidoService.delete(currentPedido.id);
      showToastMessage('Pedido excluído com sucesso!');
      setShowDeleteModal(false);
      fetchPedidos();
    } catch (error) {
      showToastMessage('Erro ao excluir o pedido.', 'failure');
      console.log(error);
    }
  };

  const getCompradorNome = (id) => compradores.find(c => c.id === id)?.nome || 'N/A';
  const getCarneDescricao = (id) => carnes.find(c => c.id === id)?.descricao || 'N/A';

  return (
    <div className="p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Gerenciamento de Pedidos</h1>
        <Button onClick={handleNewPedido}>Novo Pedido</Button>
      </div>

      {loading ? (
        <div className="text-center"><Spinner size="xl" /></div>
      ) : (
        <Table hoverable>
          <TableHead>
            <TableHeadCell>ID</TableHeadCell>
            <TableHeadCell>Comprador</TableHeadCell>
            <TableHeadCell>Data</TableHeadCell>
            <TableHeadCell>Valor Total (BRL)</TableHeadCell>
            <TableHeadCell>Ações</TableHeadCell>
          </TableHead>
          <TableBody className="divide-y">
            {pedidos.map((pedido) => (
              <TableRow key={pedido.id}>
                <TableCell>{pedido.id}</TableCell>
                <TableCell>{getCompradorNome(pedido.compradorId)}</TableCell>
                <TableCell>{new Date(pedido.dataPedido).toLocaleDateString()}</TableCell>
                <TableCell>{calculateTotalBRL(pedido.itens).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</TableCell>
                <TableCell className="flex gap-2">
                  <Button size="sm" color="blue" onClick={() => handleEdit(pedido)}>Editar</Button>
                  <Button size="sm" color="failure" onClick={() => handleDelete(pedido)}>Excluir</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      {/* Modal de Formulário */}
      <Modal show={showFormModal} size="4xl" onClose={() => setShowFormModal(false)}>
        <ModalHeader>{currentPedido?.id ? 'Editar Pedido' : 'Novo Pedido'}</ModalHeader>
        <ModalBody>
          {/* Form content aqui */}
        </ModalBody>
        <ModalFooter>
          <Button onClick={handleSave}>Salvar Pedido</Button>
          <Button color="gray" onClick={() => setShowFormModal(false)}>Cancelar</Button>
        </ModalFooter>
      </Modal>

      {/* Modal de Exclusão */}
      <Modal show={showDeleteModal} size="md" popup onClose={() => setShowDeleteModal(false)}>
        <ModalHeader />
        <ModalBody>
          <div className="text-center">
            <HiExclamation className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              Tem certeza que deseja excluir o pedido #{currentPedido?.id}?
            </h3>
            <div className="flex justify-center gap-4">
              <Button color="failure" onClick={confirmDelete}>Sim, tenho certeza</Button>
              <Button color="gray" onClick={() => setShowDeleteModal(false)}>Não, cancelar</Button>
            </div>
          </div>
        </ModalBody>
      </Modal>

      {/* Toast */}
      {toast.show && (
        <Toast className="fixed top-5 right-5 z-50">
          <div className={`inline-flex h-8 w-8 shrink-0 items-center justify-center rounded-lg ${toast.type === 'success' ? 'bg-green-100 text-green-500' : 'bg-red-100 text-red-500'}`}>
            {toast.type === 'success' ? <HiCheck className="h-5 w-5" /> : <HiX className="h-5 w-5" />}
          </div>
          <div className="ml-3 text-sm font-normal">{toast.message}</div>
          <Button size="xs" color="gray" onClick={() => setToast({ ...toast, show: false })}>Fechar</Button>
        </Toast>
      )}
    </div>
  );
}
