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
import CompradorService from '../services/CompradorService';

export default function Compradores() {
  const [compradores, setCompradores] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showFormModal, setShowFormModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [currentComprador, setCurrentComprador] = useState(null);
  const [toast, setToast] = useState({ show: false, message: '', type: 'success' });

  const estados = ['SP', 'RJ', 'MG', 'BA'];
  const cidades = {
    SP: ['São Paulo', 'Campinas', 'Santos'],
    RJ: ['Rio de Janeiro', 'Niterói', 'Petrópolis'],
    MG: ['Belo Horizonte', 'Uberlândia', 'Juiz de Fora'],
    BA: ['Salvador', 'Feira de Santana', 'Vitória da Conquista']
  };

  useEffect(() => {
    fetchCompradores();
  }, []);

  const fetchCompradores = async () => {
    setLoading(true);
    try {
      const response = await CompradorService.getAll();
      setCompradores(response.data);
    } catch (error) {
      showToastMessage('Erro ao buscar compradores.', 'failure');
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const showToastMessage = (message, type = 'success') => {
    setToast({ show: true, message, type });
    setTimeout(() => setToast({ show: false, message: '', type: 'success' }), 3000);
  };

  const handleNewComprador = () => {
    setCurrentComprador({ nome: '', documento: '', estado: 'SP', cidade: 'São Paulo' });
    setShowFormModal(true);
  };

  const handleEdit = (comprador) => {
    setCurrentComprador({ ...comprador });
    setShowFormModal(true);
  };

  const handleDelete = (comprador) => {
    setCurrentComprador(comprador);
    setShowDeleteModal(true);
  };

  const handleSave = async () => {
    if (!currentComprador.nome || !currentComprador.documento) {
      showToastMessage('Nome e Documento são obrigatórios.', 'failure');
      return;
    }

    try {
      if (currentComprador.id) {
        await CompradorService.update(currentComprador.id, currentComprador);
        showToastMessage('Comprador atualizado com sucesso!');
      } else {
        await CompradorService.create(currentComprador);
        showToastMessage('Comprador criado com sucesso!');
      }
      setShowFormModal(false);
      fetchCompradores();
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Erro ao salvar o comprador.';
      showToastMessage(errorMessage, 'failure');
    }
  };

  const confirmDelete = async () => {
    try {
      await CompradorService.delete(currentComprador.id);
      showToastMessage('Comprador excluído com sucesso!');
      setShowDeleteModal(false);
      fetchCompradores();
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Erro ao excluir. Verifique se não há pedidos associados.';
      showToastMessage(errorMessage, 'failure');
    }
  };

  return (
    <div className="p-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">Gerenciamento de Compradores</h1>
        <Button onClick={handleNewComprador}>Novo Comprador</Button>
      </div>

      {loading ? (
        <div className="text-center"><Spinner size="xl" /></div>
      ) : (
        <Table hoverable>
          <TableHead>
            <TableHeadCell>ID</TableHeadCell>
            <TableHeadCell>Nome</TableHeadCell>
            <TableHeadCell>Documento</TableHeadCell>
            <TableHeadCell>Ações</TableHeadCell>
          </TableHead>
          <TableBody className="divide-y">
            {compradores.map((comprador) => (
              <TableRow key={comprador.id}>
                <TableCell>{comprador.id}</TableCell>
                <TableCell>{comprador.nome}</TableCell>
                <TableCell>{comprador.documento}</TableCell>
                <TableCell className="flex gap-2">
                  <Button size="sm" color="blue" onClick={() => handleEdit(comprador)}>Editar</Button>
                  <Button size="sm" color="failure" onClick={() => handleDelete(comprador)}>Excluir</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      <Modal show={showFormModal} onClose={() => setShowFormModal(false)}>
        <ModalHeader>{currentComprador?.id ? 'Editar Comprador' : 'Novo Comprador'}</ModalHeader>
        <ModalBody>
          <div className="space-y-4">
            <div>
              <Label htmlFor="nome" value="Nome do Comprador" />
              <TextInput
                id="nome"
                value={currentComprador?.nome || ''}
                onChange={(e) => setCurrentComprador({ ...currentComprador, nome: e.target.value })}
                required
              />
            </div>
            <div>
              <Label htmlFor="documento" value="Documento (CPF/CNPJ)" />
              <TextInput
                id="documento"
                value={currentComprador?.documento || ''}
                onChange={(e) => setCurrentComprador({ ...currentComprador, documento: e.target.value })}
                required
              />
            </div>
            <div>
              <Label htmlFor="estado" value="Estado" />
              <Select
                id="estado"
                value={currentComprador?.estado || 'SP'}
                onChange={(e) =>
                  setCurrentComprador({
                    ...currentComprador,
                    estado: e.target.value,
                    cidade: cidades[e.target.value][0]
                  })
                }
              >
                {estados.map((uf) => (
                  <option key={uf} value={uf}>{uf}</option>
                ))}
              </Select>
            </div>
            <div>
              <Label htmlFor="cidade" value="Cidade" />
              <Select
                id="cidade"
                value={currentComprador?.cidade || ''}
                onChange={(e) => setCurrentComprador({ ...currentComprador, cidade: e.target.value })}
              >
                {currentComprador?.estado &&
                  cidades[currentComprador.estado].map((cidade) => (
                    <option key={cidade} value={cidade}>{cidade}</option>
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
              Tem certeza que deseja excluir o comprador "{currentComprador?.nome}"?
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
