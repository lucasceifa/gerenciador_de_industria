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

  const initialItemState = { carneId: '', preco: '', moeda: 'BRL' };
  const [newItem, setNewItem] = useState(initialItemState);

  useEffect(() => {
    fetchInitialData();
  }, []);

  const fetchInitialData = async () => {
    setLoading(true);
    try {
      const response = await PedidoService.getAll();
      const pedidosConvertidos = response.data.map(pedido => ({
        ...pedido,
        itens: pedido.itens.map(item => ({
          ...item,
          moeda: moedaEnumToISO[item.moeda] || 'BRL'
        }))
      }));
      const [carnesRes, compradoresRes, ratesRes] = await Promise.all([
        CarneService.getAll(),
        CompradorService.getAll(),
        CurrencyService.getRates()
      ]);
      setPedidos(pedidosConvertidos);
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
      const pedidosConvertidos = response.data.map(pedido => ({
        ...pedido,
        itens: pedido.itens.map(item => ({
          ...item,
          moeda: moedaEnumToISO[item.moeda] || 'BRL'
        }))
      }));
      setPedidos(pedidosConvertidos);
    } catch (error) {
      showToastMessage('Erro ao buscar pedidos.', 'failure');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const showToastMessage = (message, type = 'success') => {
    setToast({ show: true, message, type });
    setTimeout(() => setToast({ show: false, message: '', type: 'success' }), 6000);
  };

  const calculateTotalBRL = (itens) => {
    if (!itens || !currencyRates.USD) return 0;
    return itens.reduce((total, item) => {
      const rate = currencyRates[item.moeda] || 1;
      return total + (item.preco * rate);
    }, 0);
  };

  const handleNewPedido = () => {
    setCurrentPedido({
      dataRealizada: new Date().toISOString().split('T')[0],
      compradorId: '',
      itens: []
    });
    setShowFormModal(true);
  };

  const handleEdit = (pedido) => {
    const formattedPedido = {
      ...pedido,
      dataRealizada: new Date(pedido.dataRealizada).toISOString().split('T')[0]
    };
    setCurrentPedido(formattedPedido);
    setShowFormModal(true);
  };

  const handleDelete = (pedido) => {
    setCurrentPedido(pedido);
    setShowDeleteModal(true);
  };

  const handleAddItem = () => {
    if (!newItem.carneId || !newItem.preco || parseFloat(newItem.preco) <= 0) {
      showToastMessage('Selecione uma carne e um preço válido.', 'failure');
      return;
    }
    setCurrentPedido(prev => ({
      ...prev,
      itens: [...prev.itens, { ...newItem, preco: parseFloat(newItem.preco) }]
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
    if (
      !currentPedido.dataRealizada ||
      !currentPedido.compradorId ||
      currentPedido.itens.length === 0
    ) {
      showToastMessage('Data, comprador e ao menos um item são obrigatórios.', 'failure');
      return;
    }

    const pedidoParaSalvar = {
      ...currentPedido,
      dataRealizada: new Date(currentPedido.dataRealizada).toISOString(),
      itens: currentPedido.itens.map(item => ({
        ...item,
        moeda: moedaISOToEnum[item.moeda] || 0
      }))
    };

    try {
      if (currentPedido.id) {
        await PedidoService.update(currentPedido.id, pedidoParaSalvar);
        showToastMessage('Pedido atualizado com sucesso!');
      } else {
        await PedidoService.create(pedidoParaSalvar);
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

  const moedaEnumToISO = {
    0: 'BRL',
    1: 'USD',
    2: 'EUR'
  };

  const moedaISOToEnum = {
    BRL: 0,
    USD: 1,
    EUR: 2
  };

  const getCompradorNome = (id) => compradores.find(c => c.id === id)?.nome || 'N/A';

  return (
    <div className="p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold dark:text-white">Gerenciamento de Pedidos</h1>
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
            {pedidos?.map((pedido) => (
              <TableRow key={pedido.id} className="bg-white dark:border-gray-700 dark:bg-gray-800">
                <TableCell>{pedido.id}</TableCell>
                <TableCell>{getCompradorNome(pedido.compradorId)}</TableCell>
                <TableCell>{new Date(pedido.dataRealizada).toLocaleDateString()}</TableCell>
                <TableCell>{calculateTotalBRL(pedido.itens).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</TableCell>
                <TableCell className="flex gap-2">
                  <Button size="sm" color="blue" onClick={() => handleEdit(pedido)}>Editar</Button>
                  <Button size="sm" color="red" onClick={() => handleDelete(pedido)}>Excluir</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      <Modal show={showFormModal} size="4xl" onClose={() => setShowFormModal(false)}>
        <ModalHeader>{currentPedido?.id ? 'Editar Pedido' : 'Novo Pedido'}</ModalHeader>
        <ModalBody>
          <div className="space-y-4">
            <div>
              <label className="block mb-1 text-sm font-medium text-gray-900 dark:text-gray-300">Data do Pedido</label>
              <input
                type="date"
                value={currentPedido?.dataRealizada || ''}
                onChange={(e) =>
                  setCurrentPedido({ ...currentPedido, dataRealizada: e.target.value })
                }
                className="w-full rounded border border-gray-300 p-2"
              />
            </div>

            <div>
              <label className="block mb-1 text-sm font-medium text-gray-900 dark:text-gray-300">Comprador</label>
              <select
                value={currentPedido?.compradorId || ''}
                onChange={(e) =>
                  setCurrentPedido({ ...currentPedido, compradorId: e.target.value })
                }
                className="w-full rounded border border-gray-300 p-2"
              >
                <option value="">Selecione um comprador</option>
                {compradores.map((comprador) => (
                  <option key={comprador.id} value={comprador.id}>
                    {comprador.nome}
                  </option>
                ))}
              </select>
            </div>

            <div className="border-t border-gray-200 pt-4">
              <h3 className="text-md font-semibold mb-2 dark:text-white">Itens do Pedido</h3>

              {currentPedido?.itens?.length > 0 && (
                <ul className="space-y-2 mb-4">
                  {currentPedido.itens.map((item, index) => (
                    <li key={index} className="flex items-center justify-between bg-gray-50 dark:bg-gray-600 px-5 py-2 rounded dark:text-white">
                      <span>
                        {carnes.find(c => c.id === item.carneId)?.nome || 'N/A'} -{' '}
                        {item.preco.toLocaleString('pt-BR', {
                          style: 'currency',
                          currency: item.moeda
                        })} ({item.moeda})
                      </span>
                      <Button size="xs" color="red" onClick={() => handleRemoveItem(index)}>
                        Remover
                      </Button>
                    </li>
                  ))}
                </ul>
              )}

              <div className="grid grid-cols-3 gap-2">
                <select
                  value={newItem.carneId}
                  onChange={(e) =>
                    setNewItem({ ...newItem, carneId: e.target.value })
                  }
                  className="rounded border border-gray-300 p-2"
                >
                  <option value="">Selecione a carne</option>
                  {carnes.map((carne) => (
                    <option key={carne.id} value={carne.id}>
                      {carne.nome}
                    </option>
                  ))}
                </select>

                <input
                  type="number"
                  placeholder="Preço"
                  value={newItem.preco}
                  onChange={(e) =>
                    setNewItem({ ...newItem, preco: e.target.value })
                  }
                  className="rounded border border-gray-300 p-2"
                />

                <select
                  value={newItem.moeda}
                  onChange={(e) =>
                    setNewItem({ ...newItem, moeda: e.target.value })
                  }
                  className="rounded border border-gray-300 p-2"
                >
                  <option value="BRL">Real</option>
                  <option value="USD">Dólar</option>
                  <option value="EUR">Euro</option>
                </select>
              </div>

              <div className="mt-3">
                <Button size="sm" color="blue" onClick={handleAddItem}>
                  Adicionar Item
                </Button>
              </div>
            </div>
          </div>
        </ModalBody>
        <ModalFooter>
          <Button onClick={handleSave}>Salvar Pedido</Button>
          <Button color="gray" onClick={() => setShowFormModal(false)}>Cancelar</Button>
        </ModalFooter>
      </Modal>

      <Modal show={showDeleteModal} size="md" popup onClose={() => setShowDeleteModal(false)}>
        <ModalHeader />
        <ModalBody>
          {currentPedido && <div className="text-center">
            <HiExclamation className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              Tem certeza que deseja excluir o pedido do comprador(a) <strong>{getCompradorNome(currentPedido.compradorId)}</strong> de {calculateTotalBRL(currentPedido.itens).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })} realizado no dia {new Date(currentPedido.dataRealizada).toLocaleDateString()}?
            </h3>
            <div className="flex justify-center gap-4">
              <Button color="red" onClick={confirmDelete}>Sim, tenho certeza</Button>
              <Button color="gray" onClick={() => setShowDeleteModal(false)}>Não, cancelar</Button>
            </div>
          </div>}
        </ModalBody>
      </Modal>

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
