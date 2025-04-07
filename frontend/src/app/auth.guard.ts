import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const isAuthenticated = !!localStorage.getItem('authToken');
  if (!isAuthenticated) {
    const router = inject(Router);
    router.navigate(['/login']);
    return false;
  }
  return true;
};