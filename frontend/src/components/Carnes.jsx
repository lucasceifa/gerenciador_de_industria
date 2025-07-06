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
  TextInput,
  Label,
  Select,
  Toast,
  Spinner
} from 'flowbite-react';
import { HiCheck, HiX, HiExclamation } from 'react-icons/hi';
import CarneService from '../services/CarneService';

const origemOptions = [
  { id: 0, nome: 'Bovina' },
  { id: 1, nome: 'Suína' },
  { id: 2, nome: 'Aves' },
  { id: 3, nome: 'Peixes' },
];

export default function Carnes() {
  const [carnes, setCarnes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showFormModal, setShowFormModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [currentCarne, setCurrentCarne] = useState(null);
  const [toast, setToast] = useState({ show: false, message: '', type: 'success' });

  const fetchCarnes = async () => {
    setLoading(true);
    try {
      const response = await CarneService.getAll();
      setCarnes(response.data);
    } catch (error) {
      showToastMessage('Erro ao buscar carnes', 'failure');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const showToastMessage = (message, type = 'success') => {
    setToast({ show: true, message, type });
    setTimeout(() => setToast({ show: false, message: '', type: 'success' }), 3000);
  };

  const handleNewCarne = () => {
    setCurrentCarne({ descricao: '', origem: 0 });
    setShowFormModal(true);
  };

  const handleEdit = (carne) => {
    setCurrentCarne({ ...carne });
    setShowFormModal(true);
  };

  const handleDelete = (carne) => {
    setCurrentCarne(carne);
    setShowDeleteModal(true);
  };

  const handleSave = async () => {
    if (!currentCarne.descricao) {
      showToastMessage('A descrição da carne é obrigatória.', 'failure');
      return;
    }

    try {
      if (currentCarne.id) {
        await CarneService.update(currentCarne.id, currentCarne);
        showToastMessage('Carne atualizada com sucesso!');
      } else {
        await CarneService.create(currentCarne);
        showToastMessage('Carne criada com sucesso!');
      }
      setShowFormModal(false);
      fetchCarnes();
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Erro ao salvar a carne.';
      showToastMessage(errorMessage, 'failure');
    }
  };

  const confirmDelete = async () => {
    try {
      await CarneService.delete(currentCarne.id);
      showToastMessage('Carne excluída com sucesso!');
      setShowDeleteModal(false);
      fetchCarnes();
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Erro ao excluir. Verifique se não há pedidos associados.';
      showToastMessage(errorMessage, 'failure');
    }
  };

  const getOrigemNome = (id) => origemOptions.find(o => o.id === id)?.nome || 'Desconhecida';

  useEffect(() => {
    fetchCarnes();
  }, []);

  return (
    <div className="p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Gerenciamento de Carnes</h1>
        <Button onClick={handleNewCarne}>Nova Carne</Button>
      </div>

      {loading ? (
        <div className="text-center">
          <Spinner size="xl" />
        </div>
      ) : (
        <Table hoverable>
          <TableHead>
            <TableHeadCell>ID</TableHeadCell>
            <TableHeadCell>Descrição</TableHeadCell>
            <TableHeadCell>Origem</TableHeadCell>
            <TableHeadCell>Ações</TableHeadCell>
          </TableHead>
          <TableBody className="divide-y">
            {carnes.map((carne) => (
              <TableRow key={carne.id} className="bg-white dark:border-gray-700 dark:bg-gray-800">
                <TableCell>{carne.id}</TableCell>
                <TableCell>{carne.descricao}</TableCell>
                <TableCell>{getOrigemNome(carne.origem)}</TableCell>
                <TableCell className="flex gap-2">
                  <Button size="sm" color="blue" onClick={() => handleEdit(carne)}>Editar</Button>
                  <Button size="sm" color="failure" onClick={() => handleDelete(carne)}>Excluir</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      <Modal show={showFormModal} onClose={() => setShowFormModal(false)}>
        <ModalHeader>{currentCarne?.id ? 'Editar Carne' : 'Nova Carne'}</ModalHeader>
        <ModalBody>
          <div className="space-y-6">
            <div>
              <Label htmlFor="descricao" value="Descrição da Carne" />
              <TextInput
                id="descricao"
                value={currentCarne?.descricao || ''}
                onChange={(e) => setCurrentCarne({ ...currentCarne, descricao: e.target.value })}
                required
              />
            </div>
            <div>
              <Label htmlFor="origem" value="Origem da Carne" />
              <Select
                id="origem"
                value={currentCarne?.origem || 0}
                onChange={(e) => setCurrentCarne({ ...currentCarne, origem: parseInt(e.target.value) })}
              >
                {origemOptions.map(opt => (
                  <option key={opt.id} value={opt.id}>{opt.nome}</option>
                ))}
              </Select>
            </div>
          </div>
        </ModalBody>
        <ModalFooter>
          <Button onClick={handleSave}>Salvar</Button>
          <Button color="gray" onClick={() => setShowFormModal(false)}>Cancelar</Button>
        </ModalFooter>
      </Modal>

      <Modal show={showDeleteModal} size="md" popup onClose={() => setShowDeleteModal(false)}>
        <ModalHeader />
        <ModalBody>
          <div className="text-center">
            <HiExclamation className="mx-auto mb-4 h-14 w-14 text-gray-400 dark:text-gray-200" />
            <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
              Tem certeza que deseja excluir a carne "{currentCarne?.descricao}"?
            </h3>
            <div className="flex justify-center gap-4">
              <Button color="failure" onClick={confirmDelete}>Sim, tenho certeza</Button>
              <Button color="gray" onClick={() => setShowDeleteModal(false)}>Não, cancelar</Button>
            </div>
          </div>
        </ModalBody>
      </Modal>

      {toast.show && (
        <Toast className="fixed top-5 right-5 z-50">
          <div
            className={`inline-flex h-8 w-8 shrink-0 items-center justify-center rounded-lg ${
              toast.type === 'success'
                ? 'bg-green-100 text-green-500'
                : 'bg-red-100 text-red-500'
            }`}
          >
            {toast.type === 'success' ? (
              <HiCheck className="h-5 w-5" />
            ) : (
              <HiX className="h-5 w-5" />
            )}
          </div>
          <div className="ml-3 text-sm font-normal">{toast.message}</div>
          <Button
            size="xs"
            color="gray"
            onClick={() => setToast({ ...toast, show: false })}
          >
            Fechar
          </Button>
        </Toast>
      )}
    </div>
  );
}
