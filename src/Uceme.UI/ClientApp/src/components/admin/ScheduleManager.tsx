import React, { ReactElement, useEffect, useState } from 'react';
import {
  Button,
  Modal,
  ModalBody,
  ModalFooter,
  ModalHeader,
  Input,
  Label,
  FormGroup,
} from 'reactstrap';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import AddIcon from '@mui/icons-material/Add';
import authService from '../api-authorization/AuthorizeService';
import SettingsContext, { Settings } from '../../SettingsContext';
import './ScheduleManager.scss';

interface Turno {
  idTurno: number;
  idHospital: number;
  dia: string;
  diaSemana: string;
  inicio: string;
  fin: string;
  paralelas: number;
  porhora: number;
}

interface Hospital {
  idDatosPro: number;
  nombre: string;
  direccion: string;
}

const initialTurno: Omit<Turno, 'idTurno'> = {
  idHospital: 0,
  dia: '',
  diaSemana: '',
  inicio: '',
  fin: '',
  paralelas: 0,
  porhora: 0,
};

function ScheduleManager(): ReactElement {
  const settings: Settings = React.useContext(SettingsContext);
  const [turns, setTurns] = useState<Turno[]>([]);
  const [hospitals, setHospitals] = useState<Hospital[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedTurn, setSelectedTurn] = useState<Turno | null>(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [isAlertModalOpen, setIsAlertModalOpen] = useState(false);
  const [alertMessage, setAlertMessage] = useState('');
  const [form, setForm] = useState<Omit<Turno, 'idTurno'>>(initialTurno);
  const [isAddHospitalModalOpen, setIsAddHospitalModalOpen] = useState(false);
  const [isEditHospitalModalOpen, setIsEditHospitalModalOpen] = useState(false);
  const [isDeleteHospitalModalOpen, setIsDeleteHospitalModalOpen] =
    useState(false);
  const [selectedHospital, setSelectedHospital] = useState<Hospital | null>(
    null
  );
  const [hospitalForm, setHospitalForm] = useState<
    Omit<Hospital, 'idDatosPro'>
  >({
    nombre: '',
    direccion: '',
  });

  const fetchTurns = async () => {
    setLoading(true);
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(`${settings.baseHref}api/schedule/turns`, {
        headers: !token ? {} : { Authorization: `Bearer ${token}` },
      });
      if (!response.ok) throw new Error('Failed to fetch turns');
      const data = await response.json();
      const turnsWithWeekday = data.map((turn: Turno) => ({
        ...turn,
        diaSemana: [
          'Lunes',
          'Martes',
          'Miércoles',
          'Jueves',
          'Viernes',
          'Sábado',
          'Domingo',
        ][parseInt(turn.dia, 10) - 1],
      }));
      setTurns(turnsWithWeekday);
    } catch (err) {
      setError('Error loading turns');
    } finally {
      setLoading(false);
    }
  };

  const fetchHospitals = async () => {
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/hospitals`,
        {
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        }
      );
      if (!response.ok) throw new Error('Failed to fetch hospitals');
      const data = await response.json();
      setHospitals(data);
    } catch (err) {
      setError('Error loading hospitals');
    }
  };

  useEffect(() => {
    fetchTurns();
    fetchHospitals();
  }, []);

  const handleOpenAddModal = () => {
    setIsAddModalOpen(true);
    setForm(initialTurno);
  };

  const handleAddTurn = async () => {
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(`${settings.baseHref}api/schedule/turns`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        body: JSON.stringify(form),
      });
      if (!response.ok) throw new Error('Failed to add turn');
      await fetchTurns();
      setAlertMessage('Turn added successfully');
      setIsAlertModalOpen(true);
      setIsAddModalOpen(false);
      setForm(initialTurno);
    } catch (err) {
      setError('Error adding turn');
    }
  };

  const handleEditTurn = async () => {
    if (!selectedTurn) return;
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/turns/${selectedTurn.idTurno}`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
          },
          body: JSON.stringify(form),
        }
      );
      if (!response.ok) throw new Error('Failed to update turn');
      await fetchTurns();
      setAlertMessage('Turn updated successfully');
      setIsAlertModalOpen(true);
      setIsEditModalOpen(false);
      setForm(initialTurno);
    } catch (err) {
      setError('Error updating turn');
    }
  };

  const handleDeleteTurn = async () => {
    if (!selectedTurn) return;
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/turns/${selectedTurn.idTurno}`,
        {
          method: 'DELETE',
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        }
      );
      if (!response.ok) throw new Error('Failed to delete turn');
      await fetchTurns();
      setAlertMessage('Turn deleted successfully');
      setIsAlertModalOpen(true);
      setIsDeleteModalOpen(false);
    } catch (err) {
      setError('Error deleting turn');
    }
  };

  const handleOpenAddHospitalModal = () => {
    setIsAddHospitalModalOpen(true);
    setHospitalForm({ nombre: '', direccion: '' });
  };

  const handleAddHospital = async () => {
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/hospitals`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
          },
          body: JSON.stringify(hospitalForm),
        }
      );
      if (!response.ok) throw new Error('Failed to add hospital');
      await fetchHospitals();
      setAlertMessage('Hospital añadido correctamente');
      setIsAlertModalOpen(true);
      setIsAddHospitalModalOpen(false);
      setHospitalForm({ nombre: '', direccion: '' });
    } catch (err) {
      setError('Error adding hospital');
    }
  };

  const handleEditHospital = async () => {
    if (!selectedHospital) return;
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/hospitals/${selectedHospital.idDatosPro}`,
        {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
          },
          body: JSON.stringify(hospitalForm),
        }
      );
      if (!response.ok) throw new Error('Failed to update hospital');
      await fetchHospitals();
      setAlertMessage('Hospital actualizado correctamente');
      setIsAlertModalOpen(true);
      setIsEditHospitalModalOpen(false);
      setHospitalForm({ nombre: '', direccion: '' });
    } catch (err) {
      setError('Error updating hospital');
    }
  };

  const handleDeleteHospital = async () => {
    if (!selectedHospital) return;
    try {
      const token = await authService.getAccessToken();
      const response = await fetch(
        `${settings.baseHref}api/schedule/hospitals/${selectedHospital.idDatosPro}`,
        {
          method: 'DELETE',
          headers: !token ? {} : { Authorization: `Bearer ${token}` },
        }
      );
      if (!response.ok) throw new Error('Failed to delete hospital');
      await fetchHospitals();
      setAlertMessage('Hospital eliminado correctamente');
      setIsAlertModalOpen(true);
      setIsDeleteHospitalModalOpen(false);
    } catch (err) {
      setError('Error deleting hospital');
    }
  };

  if (loading) {
    return <div className="loading-spinner">Loading...</div>;
  }

  if (error) {
    return <div className="error-message">{error}</div>;
  }

  return (
    <div className="app app-home header-distance turnos-container">
      <div className="d-flex justify-content-between align-items-center mb-4 col-8">
        <h2 className="col-10">Gestión de Hospitales</h2>
        <div className="col-2">
          <Button color="primary" onClick={handleOpenAddHospitalModal}>
            <AddIcon /> Añadir Hospital
          </Button>
        </div>
      </div>

      <div className="table-responsive col-8">
        <table className="turns-table col-12">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Dirección</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {hospitals.map((hospital) => (
              <tr key={hospital.idDatosPro}>
                <td>{hospital.nombre}</td>
                <td>{hospital.direccion}</td>
                <td>
                  <EditIcon
                    className="clickable me-2"
                    aria-label="Editar hospital"
                    onClick={() => {
                      setSelectedHospital(hospital);
                      setHospitalForm({
                        nombre: hospital.nombre,
                        direccion: hospital.direccion,
                      });
                      setIsEditHospitalModalOpen(true);
                    }}
                  />
                  <DeleteIcon
                    className="clickable"
                    aria-label="Eliminar hospital"
                    onClick={() => {
                      setSelectedHospital(hospital);
                      setIsDeleteHospitalModalOpen(true);
                    }}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Add Hospital Modal */}
      <Modal
        isOpen={isAddHospitalModalOpen}
        toggle={() => setIsAddHospitalModalOpen(false)}
      >
        <ModalHeader toggle={() => setIsAddHospitalModalOpen(false)}>
          Añadir Hospital
        </ModalHeader>
        <ModalBody>
          <FormGroup>
            <Label for="hospitalName">Nombre</Label>
            <Input
              type="text"
              id="hospitalName"
              value={hospitalForm.nombre}
              onChange={(e) =>
                setHospitalForm((f) => ({ ...f, nombre: e.target.value }))
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="hospitalAddress">Dirección</Label>
            <Input
              type="text"
              id="hospitalAddress"
              value={hospitalForm.direccion}
              onChange={(e) =>
                setHospitalForm((f) => ({ ...f, direccion: e.target.value }))
              }
            />
          </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleAddHospital}>
            Añadir
          </Button>
          <Button
            color="secondary"
            onClick={() => setIsAddHospitalModalOpen(false)}
          >
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      {/* Edit Hospital Modal */}
      <Modal
        isOpen={isEditHospitalModalOpen}
        toggle={() => setIsEditHospitalModalOpen(false)}
      >
        <ModalHeader toggle={() => setIsEditHospitalModalOpen(false)}>
          Editar Hospital
        </ModalHeader>
        <ModalBody>
          <FormGroup>
            <Label for="hospitalName">Nombre</Label>
            <Input
              type="text"
              id="hospitalName"
              value={hospitalForm.nombre}
              onChange={(e) =>
                setHospitalForm((f) => ({ ...f, nombre: e.target.value }))
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="hospitalAddress">Dirección</Label>
            <Input
              type="text"
              id="hospitalAddress"
              value={hospitalForm.direccion}
              onChange={(e) =>
                setHospitalForm((f) => ({ ...f, direccion: e.target.value }))
              }
            />
          </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleEditHospital}>
            Guardar
          </Button>
          <Button
            color="secondary"
            onClick={() => setIsEditHospitalModalOpen(false)}
          >
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      {/* Delete Hospital Confirmation Modal */}
      <Modal
        isOpen={isDeleteHospitalModalOpen}
        toggle={() => setIsDeleteHospitalModalOpen(false)}
      >
        <ModalHeader toggle={() => setIsDeleteHospitalModalOpen(false)}>
          Confirmar Borrado
        </ModalHeader>
        <ModalBody>¿Está seguro de que desea borrar este hospital?</ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={handleDeleteHospital}>
            Borrar
          </Button>
          <Button
            color="secondary"
            onClick={() => setIsDeleteHospitalModalOpen(false)}
          >
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      <div className="d-flex justify-content-between align-items-center mb-4 col-8 mt-5">
        <h2 className="col-10">Gestión de Turnos</h2>
        <div className="col-2">
          <Button color="primary" onClick={() => handleOpenAddModal()}>
            <AddIcon /> Añadir Turno
          </Button>
        </div>
      </div>

      <div className="table-responsive col-8">
        <table className="turns-table col-12">
          <thead>
            <tr>
              <th>Hospital</th>
              <th>Día</th>
              <th>Día de la semana</th>
              <th>Inicio</th>
              <th>Fin</th>
              <th>Citas Paralelas</th>
              <th>Citas Por Hora</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {turns
              .sort(function sortTurns(a, b) {
                if (a.idHospital > b.idHospital) {
                  return 1;
                }
                if (a.idHospital < b.idHospital) {
                  return -1;
                }
                if (a.dia > b.dia) {
                  return 1;
                }
                if (a.dia < b.dia) {
                  return -1;
                }
                // a must be equal to b
                return 0;
              })
              .map((turn) => (
                <tr key={turn.idTurno}>
                  <td>
                    {
                      hospitals.find((h) => h.idDatosPro === turn.idHospital)
                        ?.nombre
                    }
                  </td>
                  <td>{turn.dia}</td>
                  <td>{turn.diaSemana}</td>
                  <td>{turn.inicio}</td>
                  <td>{turn.fin}</td>
                  <td>{turn.paralelas}</td>
                  <td>{turn.porhora}</td>
                  <td>
                    <EditIcon
                      className="clickable me-2"
                      aria-label="Editar turno"
                      onClick={() => {
                        setSelectedTurn(turn);
                        setForm({
                          idHospital: turn.idHospital,
                          dia: turn.dia,
                          diaSemana: turn.diaSemana,
                          inicio: turn.inicio,
                          fin: turn.fin,
                          paralelas: turn.paralelas,
                          porhora: turn.porhora,
                        });
                        setIsEditModalOpen(true);
                      }}
                    />
                    <DeleteIcon
                      className="clickable"
                      aria-label="Eliminar turno"
                      onClick={() => {
                        setSelectedTurn(turn);
                        setIsDeleteModalOpen(true);
                      }}
                    />
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>

      {/* Add Turn Modal */}
      <Modal isOpen={isAddModalOpen} toggle={() => setIsAddModalOpen(false)}>
        <ModalHeader toggle={() => setIsAddModalOpen(false)}>
          Añadir Turno
        </ModalHeader>
        <ModalBody>
          <FormGroup>
            <Label for="hospital">Hospital</Label>
            <Input
              type="select"
              id="hospital"
              value={form.idHospital}
              onChange={(e) =>
                setForm((f) => ({ ...f, idHospital: Number(e.target.value) }))
              }
            >
              <option value={0}>Selecciona un hospital</option>
              {hospitals.map((hospital) => (
                <option key={hospital.idDatosPro} value={hospital.idDatosPro}>
                  {hospital.nombre}
                </option>
              ))}
            </Input>
          </FormGroup>
          <FormGroup>
            <Label for="day">Día de la semana</Label>
            <Input
              type="number"
              id="day"
              value={form.dia}
              onChange={(e) => setForm((f) => ({ ...f, dia: e.target.value }))}
            />
          </FormGroup>
          <FormGroup>
            <Label for="startTime">Inicio</Label>
            <Input
              type="time"
              id="startTime"
              value={form.inicio}
              onChange={(e) =>
                setForm((f) => ({ ...f, inicio: e.target.value }))
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="endTime">Fin</Label>
            <Input
              type="time"
              id="endTime"
              value={form.fin}
              onChange={(e) => setForm((f) => ({ ...f, fin: e.target.value }))}
            />
          </FormGroup>
          <FormGroup>
            <Input
              type="number"
              id="parallel"
              value={form.paralelas}
              onChange={(e) =>
                setForm((f) => ({ ...f, paralelas: +e.target.value }))
              }
            />
            <Label check for="parallel">
              Paralelas
            </Label>
          </FormGroup>
          <FormGroup>
            <Input
              type="number"
              id="perHour"
              value={form.porhora}
              onChange={(e) =>
                setForm((f) => ({ ...f, porhora: +e.target.value }))
              }
            />
            <Label check for="perHour">
              Por Hora
            </Label>
          </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleAddTurn}>
            Añadir
          </Button>
          <Button color="secondary" onClick={() => setIsAddModalOpen(false)}>
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      {/* Edit Turn Modal */}
      <Modal isOpen={isEditModalOpen} toggle={() => setIsEditModalOpen(false)}>
        <ModalHeader toggle={() => setIsEditModalOpen(false)}>
          Editar Turno
        </ModalHeader>
        <ModalBody>
          <FormGroup>
            <Label for="hospital">Hospital</Label>
            <Input
              type="select"
              id="hospital"
              value={form.idHospital}
              onChange={(e) =>
                setForm((f) => ({ ...f, idHospital: Number(e.target.value) }))
              }
            >
              <option value={0}>Selecciona un hospital</option>
              {hospitals.map((hospital) => (
                <option key={hospital.idDatosPro} value={hospital.idDatosPro}>
                  {hospital.nombre}
                </option>
              ))}
            </Input>
          </FormGroup>
          <FormGroup>
            <Label for="day">Día de la semana</Label>
            <Input
              type="number"
              id="day"
              value={form.dia}
              onChange={(e) => setForm((f) => ({ ...f, dia: e.target.value }))}
            />
          </FormGroup>
          <FormGroup>
            <Label for="startTime">Inicio</Label>
            <Input
              type="time"
              id="startTime"
              value={form.inicio}
              onChange={(e) =>
                setForm((f) => ({ ...f, inicio: e.target.value }))
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="endTime">Fin</Label>
            <Input
              type="time"
              id="endTime"
              value={form.fin}
              onChange={(e) => setForm((f) => ({ ...f, fin: e.target.value }))}
            />
          </FormGroup>
          <FormGroup>
            <Label for="parallel">Paralelas</Label>
            <Input
              type="number"
              id="parallel"
              value={form.paralelas}
              onChange={(e) =>
                setForm((f) => ({ ...f, paralelas: +e.target.value }))
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="perHour">Por Hora</Label>
            <Input
              type="number"
              id="perHour"
              value={form.porhora}
              onChange={(e) =>
                setForm((f) => ({ ...f, porhora: +e.target.value }))
              }
            />
          </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={handleEditTurn}>
            Guardar
          </Button>
          <Button color="secondary" onClick={() => setIsEditModalOpen(false)}>
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      {/* Delete Confirmation Modal */}
      <Modal
        isOpen={isDeleteModalOpen}
        toggle={() => setIsDeleteModalOpen(false)}
      >
        <ModalHeader toggle={() => setIsDeleteModalOpen(false)}>
          Confirmar Borrado
        </ModalHeader>
        <ModalBody>¿Está seguro de que desea borrar este turno?</ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={handleDeleteTurn}>
            Borrar
          </Button>
          <Button color="secondary" onClick={() => setIsDeleteModalOpen(false)}>
            Cancelar
          </Button>
        </ModalFooter>
      </Modal>

      {/* Alert Modal */}
      <Modal
        isOpen={isAlertModalOpen}
        toggle={() => setIsAlertModalOpen(false)}
      >
        <ModalHeader toggle={() => setIsAlertModalOpen(false)}>
          Notificación
        </ModalHeader>
        <ModalBody>{alertMessage}</ModalBody>
        <ModalFooter>
          <Button color="secondary" onClick={() => setIsAlertModalOpen(false)}>
            Cerrar
          </Button>
        </ModalFooter>
      </Modal>
    </div>
  );
}

export default ScheduleManager;
