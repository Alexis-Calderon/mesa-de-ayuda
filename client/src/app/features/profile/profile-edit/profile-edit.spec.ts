import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ProfileEdit } from './profile-edit';
import { ProfileService } from '../../../core/services/profile.service';
import { AuthService } from '../../../core/services/auth.service';

describe('ProfileEdit', () => {
  let component: ProfileEdit;
  let fixture: ComponentFixture<ProfileEdit>;
  let profileServiceSpy: jasmine.SpyObj<ProfileService>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const profileSpy = jasmine.createSpyObj('ProfileService', ['getProfile', 'updateProfile']);
    const authSpy = jasmine.createSpyObj('AuthService', ['getCurrentUser', 'isAuthenticated']);
    const routerSpyObj = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [ProfileEdit],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: ProfileService, useValue: profileSpy },
        { provide: AuthService, useValue: authSpy },
        { provide: Router, useValue: routerSpyObj }
      ]
    }).compileComponents();

    profileServiceSpy = TestBed.inject(ProfileService) as jasmine.SpyObj<ProfileService>;
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileEdit);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form correctly', () => {
    expect(component.profileForm).toBeDefined();
    expect(component.profileForm.get('nombre')).toBeTruthy();
    expect(component.profileForm.get('email')).toBeTruthy();
    expect(component.profileForm.get('currentContrasenia')).toBeTruthy();
    expect(component.profileForm.get('newContrasenia')).toBeTruthy();
    expect(component.profileForm.get('confirmNewContrasenia')).toBeTruthy();
  });

  it('should load current profile on init', () => {
    const mockProfile = {
      rut: '12345678-9',
      nombre: 'Test User',
      email: 'test@example.com',
      rol: 3
    };

    profileServiceSpy.getProfile.and.returnValue(of(mockProfile));
    authServiceSpy.getCurrentUser.and.returnValue({
      rut: '12345678-9',
      nombre: 'Test User',
      email: 'test@example.com',
      rol: 3
    });

    component.ngOnInit();

    expect(profileServiceSpy.getProfile).toHaveBeenCalled();
    expect(component.profileForm.get('nombre')?.value).toBe('Test User');
    expect(component.profileForm.get('email')?.value).toBe('test@example.com');
  });

  it('should toggle password fields', () => {
    expect(component.showPasswordFields).toBeFalse();

    component.togglePasswordFields();
    expect(component.showPasswordFields).toBeTrue();

    component.togglePasswordFields();
    expect(component.showPasswordFields).toBeFalse();
  });

  it('should validate password match', () => {
    component.profileForm.patchValue({
      newContrasenia: 'NewPass123!',
      confirmNewContrasenia: 'DifferentPass123!'
    });

    expect(component.profileForm.errors?.['passwordMismatch']).toBeTruthy();
  });

  it('should update profile successfully', () => {
    const mockResponse = {
      rut: '12345678-9',
      nombre: 'Updated Name',
      email: 'updated@example.com',
      rol: 3
    };

    profileServiceSpy.updateProfile.and.returnValue(of(mockResponse));

    component.profileForm.patchValue({
      nombre: 'Updated Name',
      email: 'updated@example.com'
    });

    component.onSubmit();

    expect(profileServiceSpy.updateProfile).toHaveBeenCalled();
    expect(component.successMessage).toBe('Perfil actualizado exitosamente');
  });

  it('should handle profile update error', () => {
    profileServiceSpy.updateProfile.and.returnValue(throwError(() => new Error('Update failed')));

    component.profileForm.patchValue({
      nombre: 'Test Name',
      email: 'test@example.com'
    });

    component.onSubmit();

    expect(component.errorMessage).toBe('Error al actualizar el perfil. Inténtalo nuevamente.');
  });

  it('should get field error messages', () => {
    const nombreControl = component.profileForm.get('nombre');
    nombreControl?.markAsTouched();

    expect(component.getFieldError('nombre')).toBeTruthy();
  });

  it('should navigate back', () => {
    component.goBack();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/dashboard']);
  });
});