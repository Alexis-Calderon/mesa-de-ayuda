import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TicketService } from '../../../core/services/ticket.service';

@Component({
  selector: 'app-ticket-create',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './ticket-create.html',
  styleUrl: './ticket-create.css'
})
export class TicketCreate {
  private ticketService = inject(TicketService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  createForm: FormGroup;
  isLoading = false;

  // Opciones para los selects
  tipoOptions = [
    { value: 'Incidente', label: 'Incidente' },
    { value: 'Requerimiento', label: 'Requerimiento' },
    { value: 'Consulta', label: 'Consulta' }
  ];

  prioridadOptions = [
    { value: 'Baja', label: 'Baja' },
    { value: 'Media', label: 'Media' },
    { value: 'Alta', label: 'Alta' }
  ];

  areaOptions = [
    { value: 'Calidad', label: 'Calidad' },
    { value: 'Desarrollo', label: 'Desarrollo' },
    { value: 'Finanzas', label: 'Finanzas' },
    { value: 'Logística', label: 'Logística' },
    { value: 'Mantención', label: 'Mantención' },
    { value: 'Producción', label: 'Producción' },
    { value: 'RRHH', label: 'RRHH' },
    { value: 'Ventas', label: 'Ventas' }
  ];

  constructor() {
    this.createForm = this.fb.group({
      tipo: ['Incidente', [Validators.required]],
      prioridad: ['Media', [Validators.required]],
      area: ['', [Validators.required]],
      descripcion: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      observaciones: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]]
    });
  }

  onSubmit(): void {
    if (this.createForm.valid) {
      this.isLoading = true;
      const ticketData = this.createForm.value;

      this.ticketService.createTicket(ticketData).subscribe({
        next: (ticket) => {
          this.isLoading = false;
          alert('Ticket creado exitosamente');
          this.router.navigate(['/tickets']);
        },
        error: (error) => {
          console.error('Error creating ticket:', error);
          this.isLoading = false;
          alert('Error al crear el ticket. Por favor, inténtelo nuevamente.');
        }
      });
    } else {
      this.markFormGroupTouched(this.createForm);
    }
  }

  onCancel(): void {
    if (confirm('¿Está seguro de que desea cancelar? Se perderán los datos no guardados.')) {
      this.router.navigate(['/tickets']);
    }
  }

  private markFormGroupTouched(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      control?.markAsTouched();
    });
  }

  getErrorMessage(fieldName: string): string {
    const control = this.createForm.get(fieldName);
    if (control?.hasError('required')) {
      return 'Este campo es obligatorio';
    }
    if (control?.hasError('minlength')) {
      const minLength = control.errors?.['minlength']?.requiredLength;
      return `Debe tener al menos ${minLength} caracteres`;
    }
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength']?.requiredLength;
      return `No puede exceder ${maxLength} caracteres`;
    }
    return '';
  }
}
