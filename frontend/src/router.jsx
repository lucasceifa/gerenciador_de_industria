import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, BrowserRouter } from 'react-router-dom';
import {
  Navbar,
  NavbarBrand,
  NavbarToggle,
  NavbarCollapse,
  NavbarLink
} from 'flowbite-react';
import { Link } from 'react-router-dom';

import Carnes from './components/Carnes';
import Compradores from './components/Compradores';
import Pedidos from './components/Pedidos';

export default function AppRouter() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-100 dark:bg-gray-900 flex flex-col">
        <Navbar fluid rounded>
          <NavbarBrand as={Link} to="/">
            <span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">
              Gestão de Indústria de Carnes
            </span>
          </NavbarBrand>
          <NavbarToggle />
          <NavbarCollapse>
            <NavbarLink as={Link} to="/pedidos">
              Pedidos
            </NavbarLink>
            <NavbarLink as={Link} to="/carnes">
              Carnes
            </NavbarLink>
            <NavbarLink as={Link} to="/compradores">
              Compradores
            </NavbarLink>
          </NavbarCollapse>
        </Navbar>

        <main className="flex-1 flex items-start justify-center p-4">
          <div className="w-full max-w-6xl">
            <Routes>
              <Route path="/" element={<Navigate to="/pedidos" />} />
              <Route path="/pedidos" element={<Pedidos />} />
              <Route path="/carnes" element={<Carnes />} />
              <Route path="/compradores" element={<Compradores />} />
              <Route path="*" element={<Navigate to="/pedidos" replace />} />
            </Routes>
          </div>
        </main>
      </div>
    </BrowserRouter>
  );
}
