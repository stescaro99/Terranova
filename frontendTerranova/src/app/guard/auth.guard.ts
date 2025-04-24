import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserService } from '../services/user.service';

export const authGuard:  CanActivateFn = async (route, state) => {
  const userService = inject(UserService);
  const router = inject(Router);

  const isAuthenticated = await new Promise<boolean>((resolve) => {
    setTimeout(() => {
      const token = !!localStorage.getItem('authToken');
      resolve(token && userService.isUserValid());
    }, 100);
  });
  if (!isAuthenticated) {
    router.navigate(['/login']);
    return false;
  }
  return true;
};